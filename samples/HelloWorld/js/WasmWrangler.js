class Wasm {
    static callStaticMethod(assemblyName, type, methodName, ...args) {
        BINDING.call_static_method(`[${assemblyName}] ${type}:${methodName}`);
    }
}
//# sourceMappingURL=WasmWrangler.js.map