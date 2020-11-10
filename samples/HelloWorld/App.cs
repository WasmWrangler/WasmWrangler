using System;
using WebAssembly;

namespace HelloWorld
{
    public static class App
    {
        private static JSObject? _app;
        private static int _clickCount = 0;

        public static void WasmMain()
        {
            Console.WriteLine("Hello World from WASM!");

            _app = (JSObject)Runtime.GetGlobalObject("App");
            _app.Invoke("sayHello", "smack0007");
        }

        public static void IncrementClickCount()
        {
            _clickCount++;

            if (_app != null)
                _app.SetObjectProperty("clickCount", _clickCount);
        }
    }
}
