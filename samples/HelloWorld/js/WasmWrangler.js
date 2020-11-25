"use strict";
var WasmStaticClassContext = /** @class */ (function () {
    function WasmStaticClassContext(assemblyName, type) {
        this._invokePrefix = "[" + assemblyName + "]" + type + ":";
    }
    WasmStaticClassContext.prototype.invokeMethod = function (methodName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        BINDING.call_static_method("" + this._invokePrefix + methodName, args);
    };
    return WasmStaticClassContext;
}());
var Wasm = /** @class */ (function () {
    function Wasm() {
    }
    Wasm.invokeStaticMethod = function (assemblyName, type, methodName) {
        var args = [];
        for (var _i = 3; _i < arguments.length; _i++) {
            args[_i - 3] = arguments[_i];
        }
        BINDING.call_static_method("[" + assemblyName + "]" + type + ":" + methodName, args);
    };
    Wasm.createStaticClassContext = function (assmeblyName, type) {
        return new WasmStaticClassContext(assmeblyName, type);
    };
    return Wasm;
}());
//# sourceMappingURL=WasmWrangler.js.map