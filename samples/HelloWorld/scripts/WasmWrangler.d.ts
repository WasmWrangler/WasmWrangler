declare var BINDING: any;
declare class Wasm {
    static callStaticMethod(assemblyName: string, type: string, methodName: string, ...args: any[]): any;
}
