using System.Collections.Generic;

namespace WasmWrangler.BindingGenerator
{
    public class InterfaceInfo
    {
        public string Name { get; init; } = "";

        public List<string> Extends { get; } = new();

        public List<PropertyInfo> Properties { get; } = new();

        public List<MethodInfo> Methods { get; } = new();

        public override string ToString() => Name;
    }
}
