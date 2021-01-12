using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmWrangler.BindingGenerator
{
    public class SyntaxNode
    {
        public int pos { get; set; }
        
        public int end { get; set; }

        public string kind { get; set; } = "";

        public string escapedText { get; set; } = "";

        public SyntaxNode? name { get; set; } = null;

        public List<SyntaxNode>? statements { get; set; } = null;

        public List<SyntaxNode>? members { get; set; } = null;

        public override string ToString()
        {
            return $"[{kind}:{pos}:{end}] {escapedText}";
        }
    }
}
