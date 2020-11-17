using System;

namespace WasmWrangler.BindingGenerator
{
    public class WasmWranglerBinding
    {
        public string Name { get; set; } = "";

        public WasmWranglerMethodBinding[] Methods { get; set; } = Array.Empty<WasmWranglerMethodBinding>();

        public override string ToString() => Name;
    }
}
