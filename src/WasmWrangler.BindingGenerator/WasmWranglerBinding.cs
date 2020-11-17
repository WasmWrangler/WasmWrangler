using System;

namespace WasmWrangler.BindingGenerator
{
    public class WasmWranglerBinding
    {
        public string Name { get; set; } = "";

        public string Type { get; set; } = "";

        public WasmWranglerPropertyBinding[] Properties { get; set; } = Array.Empty<WasmWranglerPropertyBinding>();

        public WasmWranglerMethodBinding[] Methods { get; set; } = Array.Empty<WasmWranglerMethodBinding>();

        public override string ToString() => Name;
    }
}
