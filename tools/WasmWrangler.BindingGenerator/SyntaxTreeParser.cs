using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmWrangler.BindingGenerator
{
    public static class SyntaxTreeParser
    {
        private static string GetLocation(SyntaxNode node) => $"{{{node.id}:{node.pos}-{node.end}}}";

        private static void EnsureKind(SyntaxNode node, params string[] kinds)
        {
            if (!kinds.Contains(node.kind))
                throw new InvalidOperationException($"Expected {string.Join(" | ", kinds)} at node {GetLocation(node)}.");
        }

        public static void Parse(Context context, SyntaxNode node)
        {
            EnsureKind(node, SyntaxNodeKind.SourceFile);

            foreach (var statement in node.statements!)
            {
                ParseStatement(context, statement);
            }
        }

        private static void ParseStatement(Context context, SyntaxNode node)
        {
            switch (node.kind)
            {
                case SyntaxNodeKind.InterfaceDeclaration:
                    ParseInterfaceDeclaration(context, node);
                    break;
            }
        }

        private static void ParseInterfaceDeclaration(Context context, SyntaxNode node)
        {
            var @interface = new InterfaceInfo()
            {
                Name = node.name!.escapedText,
            };

            if (node.heritageClauses != null)
            {
                foreach (var heritageClause in node.heritageClauses)
                {
                    foreach (var type in heritageClause.types!)
                    {
                        EnsureKind(type, SyntaxNodeKind.ExpressionWithTypeArguments);
                        @interface.Extends.Add(type.expression!.escapedText);
                    }
                }
            }

            context.Interfaces.Add(@interface);
        }
    }
}
