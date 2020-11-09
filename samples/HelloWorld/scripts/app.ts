import { greet } from "./greeter";

declare var BINDING: any;

const App = {
    init: function () {
        document.getElementById("loading").style.display = 'none';
        document.getElementById("app").style.display = 'block';
        BINDING.call_static_method("[HelloWorld] HelloWorld.Program:WasmMain");
    },

    sayHello: function (name) {
        console.info(greet(name));
    }
};
