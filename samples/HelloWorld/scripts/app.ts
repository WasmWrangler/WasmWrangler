/// <reference path="greeter.ts" />

declare var BINDING: any;

const App = {
    clickCount: 0,

    init: function () {
        document.getElementById("loading").style.display = 'none';
        document.getElementById("app").style.display = 'block';
        BINDING.call_static_method("[HelloWorld] HelloWorld.App:WasmMain");
    },

    sayHello: function (name: string): void {
        console.info(greet(name));
    },

    onClick: function (button: HTMLButtonElement) {
        BINDING.call_static_method("[HelloWorld] HelloWorld.App:IncrementClickCount");
        button.innerText = `Clicked ${App.clickCount} times`;
    }
};
