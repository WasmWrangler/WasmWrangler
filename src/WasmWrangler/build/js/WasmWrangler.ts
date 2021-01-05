interface BINDING {
    bindings_lazy_init(): void;

    assembly_load(name: string): number;

    call_method(method: number, thisArg: number | null, signature: any, args?: any[]): any;

    call_static_method(fqn: string, args?: any[]): any;

    find_class(assemblyHandle: number, namespace: string, name: string): number;

    find_method(classHandle: number, name: string, _: number): number;
}

declare var BINDING: BINDING;

interface MONO {
    _load_assets_and_runtime: (args: any) => any;

    mono_load_runtime_and_bcl(
        vfs_prefix: string,
        deploy_prefix: string,
        enable_debugging: number,
        file_list: string[],
        loadedCallback: () => void,
        fetchCallback: (asset: string) => Promise<any>,
    ): void;
}

declare var MONO: MONO;

interface ModuleRuntime {
    mono_method_get_call_signature(methodHandle: number): any;
}

var Module = {
    // This function will be called by dotnet.js when the runtime is ready.
    onRuntimeInitialized: function () {
        WasmWrangler._onRuntimeInitialized();
    }
} as any as ModuleRuntime; // dotnet.js will also add functions to the Module object which we want to call later.

class WasmWranglerAssemblyReference {
    private _loaded: boolean = false;
    private _handle: number = 0;

    constructor(private _fileName: string) {
    }

    public get loaded(): boolean {
        return this._loaded;
    }

    public get handle(): number {
        return this._handle;
    }

    public get fileName(): string {
        return this._fileName;
    }

    public get name(): string {
        let name = this._fileName;

        if (name.endsWith(".dll")) {
            name = name.substring(0, name.length - ".dll".length);
        }

        return name;
    }

    public getClass(namespace: string, name: string) {
        if (this._handle === 0) {
            throw new Error("Assembly reference not yet fully initialized.");
        }

        const classHandle = BINDING.find_class(this._handle, namespace, name);

        if (classHandle === 0) {
            throw new Error(`Unable to find class ${namespace}.${name} in assembly ${this.name}`);
        }

        return new WasmWranglerClassReference(this, classHandle, namespace, name);
    }
}

class WasmWranglerClassReference {
    constructor(
        private _assembly: WasmWranglerAssemblyReference,
        private _handle: number,
        private _namespace: string,
        private _name: string,
    ) {
    }

    public get assembly(): WasmWranglerAssemblyReference {
        return this._assembly;
    }

    public get namespace(): string {
        return this._namespace;
    }

    public get name(): string {
        return this._name;
    }

    public get fullName(): string {
        return this._namespace + "." + this._name;
    }

    public invokeStaticMethod(methodName: string, ...args: any[]) {
        const methodHandle = BINDING.find_method(this._handle, methodName, - 1);

        if (methodHandle === 0) {
            throw new Error(`Unable to find method ${methodName} in class ${this.fullName}`);
        }

        const signature = Module.mono_method_get_call_signature(methodHandle);

        return BINDING.call_method(methodHandle, null, signature, args);
    }
}

interface WasmWranglerConfig {
    vfs_prefix: string;
    deploy_prefix: string;
    enable_debugging: number;
    file_list: string[];
}

interface WasmWranglerCallbacks {
    onLoaded: () => void;

    onLoadProgress?: (loadedAssembliesCount: number, totalAssembliesCount: number) => void;
}

class WasmWrangler {
    // This will be filled in the final output for a project.
    private static _config: WasmWranglerConfig;

    private static _runtimeInitialized: boolean = false;
    private static _initialized: boolean = false;
    private static _callbacks: WasmWranglerCallbacks | undefined = undefined; 

    public static _assemblies: WasmWranglerAssemblyReference[] = [];

    public static get assemblies(): WasmWranglerAssemblyReference[] {
        return [...WasmWrangler._assemblies];
    }

    public static _onRuntimeInitialized(): void {
        // TODO: We should hide this behind some configuration flag.
        //const _load_assets_and_runtime = MONO._load_assets_and_runtime;
        //MONO._load_assets_and_runtime = (args: any) => {
        //    args.diagnostic_tracing = true;
        //    _load_assets_and_runtime.apply(MONO, [args]);
        //};

        WasmWrangler._runtimeInitialized = true;
        if (!WasmWrangler._initialized && WasmWrangler._callbacks !== undefined) {
            WasmWrangler._doInitialize();
        }
    }

    private static _doInitialize() {
        if (!WasmWrangler._runtimeInitialized) {
            return;
        }

        const config = WasmWrangler._config;

        if (config === undefined) {
            throw new Error("WasmWrangler._config was undefined.");
        }

        WasmWrangler._assemblies = [];

        for (const fileName of config.file_list) {
            this._assemblies.push(new WasmWranglerAssemblyReference(fileName));
        }

        MONO.mono_load_runtime_and_bcl(
            config.vfs_prefix,
            config.deploy_prefix,
            config.enable_debugging,
            config.file_list,
            () => {
                BINDING.bindings_lazy_init();

                for (const assembly of this._assemblies) {
                    (assembly as any)["_handle"] = BINDING.assembly_load(assembly.name);
                }

                WasmWrangler._initialized = true;
                if (WasmWrangler._callbacks !== undefined) {
                    WasmWrangler._callbacks.onLoaded();
                }
            },
            (asset: string) => {
                return fetch(asset, { credentials: 'same-origin' })
                    .then((value) => {
                        const fileName = WasmWrangler.getFileName(asset);

                        const assembly = WasmWrangler._assemblies.find((x => x.fileName === fileName));
                        (assembly as any)["_loaded"] = true;

                        if (WasmWrangler._callbacks !== undefined && WasmWrangler._callbacks.onLoadProgress !== undefined) {
                            WasmWrangler._callbacks.onLoadProgress(WasmWrangler._assemblies.filter(x => x.loaded).length, WasmWrangler._assemblies.length);
                        }

                        return value;
                    });
            }
        );
    }

    public static initialize(callbacks: (() => void) | WasmWranglerCallbacks): void {
        if (WasmWrangler._initialized || WasmWrangler._callbacks !== undefined) {
            throw new Error("WasmWrangler.initialize() has already been called.");
        }

        if (typeof callbacks === "function") {
            callbacks = {
                onLoaded: callbacks
            };
        }

        WasmWrangler._callbacks = callbacks;
        WasmWrangler._doInitialize();
    }

    public static getAssembly(name: string): WasmWranglerAssemblyReference | undefined {
        return WasmWrangler._assemblies.find(x => x.name === name);
    }

    public static invokeStaticMethod(
        assemblyName: string,
        type: string,
        methodName: string,
        ...args: any[]): any {
        return BINDING.call_static_method(`[${assemblyName}]${type}:${methodName}`, args);
    }

    public static getFileName(url: string): string {
        const parts = url.split("/");

        if (parts.length > 0) {
            return parts[parts.length - 1];
        }

        return "";
    }
};

