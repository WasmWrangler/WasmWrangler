using System.Collections.Generic;

namespace TypeScriptGenerator
{
    public class SyntaxNode
    {
        public int id { get; set; }

        public int pos { get; set; }
        
        public int end { get; set; }

        public string kind { get; set; } = "";

        public string escapedText { get; set; } = "";

        public SyntaxNode? expression { get; set; } = null;

        public List<SyntaxNode>? heritageClauses { get; set; } = null;

        public List<SyntaxNode>? jsDoc { get; set; } = null;

        public List<SyntaxNode>? members { get; set; } = null;
        
        public SyntaxNode? name { get; set; } = null;
        
        public SyntaxNode? questionToken { get; set; } = null;

        public List<SyntaxNode>? statements { get; set; } = null;

        public SyntaxNode? type { get; set; } = null;

        public List<SyntaxNode>? types { get; set; } = null;

        public override string ToString()
        {
            return $"[{id}:{kind}:{pos}-{end}] {escapedText}";
        }
    }
}
