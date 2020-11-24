"use strict";
var Wasm = /** @class */ (function () {
    function Wasm() {
    }
    Wasm.callStaticMethod = function (assemblyName, type, methodName) {
        var args = [];
        for (var _i = 3; _i < arguments.length; _i++) {
            args[_i - 3] = arguments[_i];
        }
        BINDING.call_static_method("[" + assemblyName + "] " + type + ":" + methodName);
    };
    return Wasm;
}());
//# sourceMappingURL=WasmWrangler.js.map