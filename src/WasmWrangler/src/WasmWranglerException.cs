using System;

namespace WasmWrangler
{
    public class WasmWranglerException : Exception
    {
        public WasmWranglerException(string message)
            : base(message)
        {
        }
    }
}
