using System;
using WebAssembly;

namespace HelloWorld
{
    public static class Program
    {
        private static JSObject? _js;
        private static int _clickCount = 0;

        public static void Main()
        {
            Console.WriteLine("Hello World from WASM!");

            _js = (JSObject)Runtime.GetGlobalObject("Program");
            _js.Invoke("onReady");
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
