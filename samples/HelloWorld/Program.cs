using System;
using WebAssembly;
using WasmWrangler;

namespace HelloWorld
{
    public static class Program
    {
        private static JSObject? _js;
        private static int _clickCount = 0;

        public static void Main()
        {
            Console.WriteLine("Hello World from WASM!");

            var loading = JS.document.getElementById("loading");

            if (loading != null)
            {
                Console.WriteLine(loading.innerText);
                loading.style.SetObjectProperty("display", "none");
            }

            var app = JS.document.getElementById("app");

            if (app != null)
            {
                app.style.SetObjectProperty("display", "block");
            }

            _js = (JSObject)Runtime.GetGlobalObject("Program");
            _js.Invoke("sayHello", "smack0007");
        }

        public static void IncrementClickCount()
        {
            _clickCount++;

            if (_js != null)
                _js.SetObjectProperty("clickCount", _clickCount);
        }
    }
}
