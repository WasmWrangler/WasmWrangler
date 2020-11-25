declare var BINDING: any;

class WasmStaticClassContext {
    private _invokePrefix: string;

    constructor(assemblyName: string, type: string) {
        this._invokePrefix = `[${assemblyName}]${type}:`;
    }

    public invokeMethod(methodName: string, ...args: any[]) {
        BINDING.call_static_method(`${this._invokePrefix}${methodName}`, args);
    }
}

class Wasm {
    public static invokeStaticMethod(
        assemblyName: string,
        type: string,
        methodName: string,
        ...args: any[]): any {
        BINDING.call_static_method(`[${assemblyName}]${type}:${methodName}`, args);
    }

    public static createStaticClassContext(assmeblyName: string, type: string): WasmStaticClassContext {
        return new WasmStaticClassContext(assmeblyName, type);
    }
}
