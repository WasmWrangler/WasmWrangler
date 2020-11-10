/// <reference path="greeter.ts" />

declare var BINDING: any;

class App {
    public static clickCount: number = 0;

    public static init(): void {
        document.getElementById("loading").style.display = 'none';
        document.getElementById("app").style.display = 'block';
        BINDING.call_static_method("[HelloWorld] HelloWorld.App:WasmMain");
    }

    public static sayHello(name: string): void {
        console.info(greet(name));
    }

    public static onClick(button: HTMLButtonElement): void {
        BINDING.call_static_method("[HelloWorld] HelloWorld.App:IncrementClickCount");
        button.innerText = `Clicked ${App.clickCount} times`;
    }
}
