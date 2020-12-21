declare var BINDING: any;
declare var MONO: any;

var Module = {
    onRuntimeInitialized: function () {
        WasmWrangler._onRuntimeInitialized();
    }
};

class WasmWranglerAssemblyReference {
    private _loaded: boolean = false;

    constructor(private _fileName: string) {
    }

    public get loaded(): boolean {
        return this._loaded;
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
}

class WasmStaticClassContext {
    private _invokePrefix: string;

    constructor(assemblyName: string, type: string) {
        this._invokePrefix = `[${assemblyName}]${type}:`;
    }

    public invokeMethod(methodName: string, ...args: any[]) {
        BINDING.call_static_method(`${this._invokePrefix}${methodName}`, args);
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

    onAssemblyLoaded?: (assembly: string) => void;
}

class WasmWrangler {
    // This will be filled in the final output for a project.
    private static _config: WasmWranglerConfig;

    private static _runtimeInitialized: boolean = false;
    private static _initialized: boolean = false;
    private static _callbacks: WasmWranglerCallbacks | undefined = undefined; 

    public static assemblies: { [key: string]: WasmWranglerAssemblyReference } = {};

    public static _onRuntimeInitialized(): void {
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

        WasmWrangler.assemblies = {};

        for (const fileName of config.file_list) {
            this.assemblies[fileName] = new WasmWranglerAssemblyReference(fileName);
        }

        MONO.mono_load_runtime_and_bcl(
            config.vfs_prefix,
            config.deploy_prefix,
            config.enable_debugging,
            config.file_list,
            () => {
                WasmWrangler._initialized = true;
                if (WasmWrangler._callbacks !== undefined) {
                    WasmWrangler._callbacks.onLoaded();
                }
            },
            (asset: string) => {
                return fetch(asset, { credentials: 'same-origin' })
                    .then((value) => {
                        const fileName = WasmWrangler.getFileName(asset);

                        (WasmWrangler.assemblies[fileName] as any)["_loaded"] = true;

                        if (WasmWrangler._callbacks !== undefined && WasmWrangler._callbacks.onAssemblyLoaded !== undefined) {
                            WasmWrangler._callbacks.onAssemblyLoaded(fileName);
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

    public static createStaticClassContext(assmeblyName: string, type: string): WasmStaticClassContext {
        return new WasmStaticClassContext(assmeblyName, type);
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

