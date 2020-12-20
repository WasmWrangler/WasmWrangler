declare var BINDING: any;
declare var MONO: any;

var Module = {
    onRuntimeInitialized: function () {
        WasmWrangler._onRuntimeInitialized();
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
    _ready: false,
    _onReadyCallbacks: [] as (() => void)[],

    _config: {
        vfs_prefix: "managed",
        deploy_prefix: "managed",
        enable_debugging: 0,
        file_list: [] as string[],
    },

    _onRuntimeInitialized: function (): void {
        MONO.mono_load_runtime_and_bcl(
            this._config.vfs_prefix,
            this._config.deploy_prefix,
            this._config.enable_debugging,
            this._config.file_list,
            () => {
                this._ready = true;

                for (const callback of this._onReadyCallbacks) {
                    callback();
                }

                this._onReadyCallbacks = [];
            },
            function (asset: string) {
                console.info("Fetching " + asset);
                return fetch(asset, { credentials: 'same-origin' });
            }
        );
    },

    createStaticClassContext: function (assmeblyName: string, type: string): WasmStaticClassContext {
        return new WasmStaticClassContext(assmeblyName, type);
    },

    invokeStaticMethod: function (
        assemblyName: string,
        type: string,
        methodName: string,
        ...args: any[]): any {
        return BINDING.call_static_method(`[${assemblyName}]${type}:${methodName}`, args);
    },

    onReady: function (callback: () => void) {
        if (!this._ready) {
            this._onReadyCallbacks.push(callback);
            return;
        }

        callback();
    }
};

