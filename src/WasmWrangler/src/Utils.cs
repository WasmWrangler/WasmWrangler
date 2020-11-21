using System;
using WebAssembly;

namespace WasmWrangler
{
    internal class Utils
    {
        public static T? Cast<T>(object obj)
            where T: class
        {
            if (obj is null)
                return null;

            if (obj is JSObject)
            {
                Console.WriteLine("obj is JSObject");
                return ((JSObject)obj) as T;
            }

            return null;
        }
    }
}
