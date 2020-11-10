/// <reference path="greeter.ts" />

declare var Wasm: any;

class App {
    public static clickCount: number = 0;

    public static init(): void {
        document.getElementById("loading").style.display = 'none';
        document.getElementById("app").style.display = 'block';
        Wasm.callStaticMethod("HelloWorld", "HelloWorld.App", "WasmMain");
    }

    public static sayHello(name: string): void {
        console.info(greet(name));
    }

    public static onClick(button: HTMLButtonElement): void {
        Wasm.callStaticMethod("HelloWorld", "HelloWorld.App", "IncrementClickCount");
        button.innerText = `Clicked ${App.clickCount} times`;
    }
}
