using System;
using WasmWrangler;

namespace HelloWorld
{
    public static class Program
    {
        private static int _clickCount = 0;

        public static void Main()
        {
            Console.WriteLine("Hello World from WASM!");

            var loading = JS.document.getElementById("loading");
            loading!.style.display = "none";

            var app = JS.document.getElementById("app");
            app!.style.display = "block";
        }

        public static void IncrementClickCount()
        {
            _clickCount++;

            var button = JS.document.getElementById("button");
            button!.innerText = $"Clicked {_clickCount} times";
        }
    }
}
