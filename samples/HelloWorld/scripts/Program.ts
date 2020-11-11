/// <reference path="WasmWrangler.ts" />
/// <reference path="Greeter.ts" />

class Program {
    public static clickCount: number = 0;

    public static onReady(): void {
        document.getElementById("loading").style.display = 'none';
        document.getElementById("app").style.display = 'block';
    }

    public static sayHello(name: string): void {
        Greeter.greet(name);
    }

    public static onClick(button: HTMLButtonElement): void {
        Wasm.callStaticMethod("HelloWorld", "HelloWorld.Program", "IncrementClickCount");
        button.innerText = `Clicked ${Program.clickCount} times`;
    }
}
