using System;
using WasmWrangler.Interop.Browser;

namespace HelloWorld
{
    public static class Program
    {
        private static int _clickCount = 0;

        public static void Main()
        {
            Console.WriteLine("Hello World from WASM!");

            var loading = document.getElementById("loading");
            loading!.style.display = "none";

            var app = document.getElementById("app");
            app!.style.display = "block";
        }

        public static void IncrementClickCount()
        {
            _clickCount++;

            var button = document.getElementById("button");
            button!.innerText = $"Clicked {_clickCount} times";
        }
    }
}
