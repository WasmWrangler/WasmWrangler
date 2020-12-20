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

interface WasmWranglerConfig {
    vfs_prefix: string;
    deploy_prefix: string;
    enable_debugging: number;
    file_list: string[];
}

class WasmWrangler {
    // This will be filled in the final output for a project.
    private static _config: WasmWranglerConfig;

    private static _ready: boolean = false;
    private static _onReady: (() => void) | undefined = undefined; 

    public static _onRuntimeInitialized(): void {

        MONO.mono_load_runtime_and_bcl(
            this._config.vfs_prefix,
            this._config.deploy_prefix,
            this._config.enable_debugging,
            this._config.file_list,
            () => {
                WasmWrangler._ready = true;
                if (WasmWrangler._onReady !== undefined) {
                    WasmWrangler._onReady();
                }
            },
            (asset: string) => {
                console.info("Fetching " + asset);
                return fetch(asset, { credentials: 'same-origin' });
            }
        );
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

    public static get onReady(): (() => void) | undefined {
        return WasmWrangler._onReady;
    }

    public static set onReady(callback: (() => void) | undefined) {
        WasmWrangler._onReady = callback;
        if (WasmWrangler._ready && callback !== undefined) {
            callback();
        }
    }
};

