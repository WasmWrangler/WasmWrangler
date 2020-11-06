using System;
using WebAssembly;

namespace HelloWorld
{
    class Program
    {
        static void WasmMain()
        {
            Console.WriteLine("Hello World from WASM!");

            var app = (JSObject)Runtime.GetGlobalObject("App");
            app.Invoke("sayHello", "smack0007");
        }
    }
}
