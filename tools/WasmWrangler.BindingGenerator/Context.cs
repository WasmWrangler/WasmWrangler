using System.Collections.Generic;

namespace WasmWrangler.BindingGenerator
{
    public class Context
    {
        public string InputFileName { get; init; } = "";

        public List<InterfaceInfo> Interfaces { get; } = new List<InterfaceInfo>();
    }
}
