declare var BINDING: any;
declare var MONO: any;

var Module = {
    onRuntimeInitialized: function () {
        console.info("Module initialized", WasmWrangler.config);
        MONO.mono_load_runtime_and_bcl(
            WasmWrangler.config.vfs_prefix,
            WasmWrangler.config.deploy_prefix,
            WasmWrangler.config.enable_debugging,
            WasmWrangler.config.file_list,
            function () {
                console.info("WasmWrangler initialized");
                BINDING.call_static_method("[HelloWorld] HelloWorld.Program:Main");
            },
            function (asset: string) {
                console.info("Fetching " + asset);
                return fetch(asset, { credentials: 'same-origin' });
            }
        );
    }
};

class WasmStaticClassContext {
    private _invokePrefix: string;

    constructor(assemblyName: string, type: string) {
        this._invokePrefix = `[${assemblyName}]${type}:`;
    }

    public invokeMethod(methodName: string, ...args: any[]) {
        BINDING.call_static_method(`${this._invokePrefix}${methodName}`, args);
    }
}

var WasmWrangler = {
    config: {
        vfs_prefix: "managed",
        deploy_prefix: "managed",
        enable_debugging: 0,
        file_list: [] as string[],
    },

    onRuntimeInitialized: function (): void {
        MONO.mono_load_runtime_and_bcl(
            WasmWrangler.config.vfs_prefix,
            WasmWrangler.config.deploy_prefix,
            WasmWrangler.config.enable_debugging,
            WasmWrangler.config.file_list,
            function () {
                console.info("WasmWrangler initialized");
                // BINDING.call_static_method("[HelloWorld] HelloWorld.Program:Main");
            },
            function (asset: string) {
                console.info("Fetching " + asset);
                return fetch(asset, { credentials: 'same-origin' });
            }
        );
    },

    invokeStaticMethod: function (
        assemblyName: string,
        type: string,
        methodName: string,
        ...args: any[]): any {
        return BINDING.call_static_method(`[${assemblyName}]${type}:${methodName}`, args);
    },

    createStaticClassContext: function (assmeblyName: string, type: string): WasmStaticClassContext {
        return new WasmStaticClassContext(assmeblyName, type);
    }
};

