using System;

namespace WasmWrangler.BindingGenerator
{
    public class WasmWranglerMethodBinding
    {
        public string Name { get; set; } = "";

        public WasmWranglerMethodArgBinding[] Args { get; set; } = Array.Empty<WasmWranglerMethodArgBinding>();

        public string ReturnType { get; set; } = "void";
    }
}
