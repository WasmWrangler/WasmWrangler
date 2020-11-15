﻿/// <reference path="WasmWrangler.d.ts" />
/// <reference path="Greeter.ts" />

class Program {
    public static clickCount: number = 0;

    public static sayHello(name: string): void {
        Greeter.greet(name);
    }

    public static onClick(button: HTMLButtonElement): void {
        Wasm.callStaticMethod("HelloWorld", "HelloWorld.Program", "IncrementClickCount");
        button.innerText = `Clicked ${Program.clickCount} times`;
    }
}