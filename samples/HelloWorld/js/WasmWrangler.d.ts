declare var BINDING: any;
declare class WasmStaticClassContext {
    private _invokePrefix;
    constructor(assemblyName: string, type: string);
    invokeMethod(methodName: string, ...args: any[]): void;
}
declare class Wasm {
    static invokeStaticMethod(assemblyName: string, type: string, methodName: string, ...args: any[]): any;
    static createStaticClassContext(assmeblyName: string, type: string): WasmStaticClassContext;
}
