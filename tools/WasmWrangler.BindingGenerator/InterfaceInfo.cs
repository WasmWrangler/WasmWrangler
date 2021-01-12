using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmWrangler.BindingGenerator
{
    public class InterfaceInfo
    {
        public string Name { get; init; } = "";

        public List<string> Extends { get; } = new List<string>();

        public override string ToString() => Name;
    }
}
