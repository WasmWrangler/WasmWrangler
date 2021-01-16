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

            var canvas = document.getElementById<HTMLCanvasElement>("canvas");
            canvas!.width = 400;
            canvas!.height = 300;

            var context = canvas!.getContext<CanvasRenderingContext2D>("2d");
            context!.fillStyle = "black";
            context!.fillRect(0, 0, 400, 300);
            context!.beginPath();
            context!.moveTo(0, 0);
            context!.lineTo(400, 300);
            context!.strokeStyle = "red";
            context!.stroke();
        }

        public static void IncrementClickCount()
        {
            _clickCount++;

            var button = document.getElementById("button");
            button!.innerText = $"Clicked {_clickCount} times";
        }
    }
}
