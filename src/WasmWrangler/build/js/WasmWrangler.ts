declare var BINDING: any;

class Wasm {
    public static callStaticMethod(
        assemblyName: string,
        type: string,
        methodName: string,
        ...args: any[]): any {
        BINDING.call_static_method(`[${assemblyName}] ${type}:${methodName}`);
    }
}
