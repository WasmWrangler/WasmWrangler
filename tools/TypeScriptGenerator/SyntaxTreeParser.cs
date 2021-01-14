using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TypeScriptGenerator
{
    public static class SyntaxTreeParser
    {
        private static string GetLocation(SyntaxNode node) => $"{{{node.id}:{node.pos}-{node.end}}}";

        private static void EnsureKind(SyntaxNode node, params string[] kinds)
        {
            if (!kinds.Contains(node.kind))
                throw new InvalidOperationException($"Expected {string.Join(" | ", kinds)} at node {GetLocation(node)}.");
        }

        private static void EnsureNotNull(SyntaxNode parent, [NotNull] SyntaxNode? node, string name)
        {
            if (node == null)
                throw new InvalidOperationException($"Expected {name} not to be null at node {GetLocation(parent)}.");
        }

        private static void EnsureNotNull(SyntaxNode parent, [NotNull] List<SyntaxNode>? node, string name)
        {
            if (node == null)
                throw new InvalidOperationException($"Expected {name} not to be null at node {GetLocation(parent)}.");
        }

        public static void Parse(Context context, SyntaxNode node)
        {
            EnsureKind(node, SyntaxNodeKind.SourceFile);
            EnsureNotNull(node, node.statements, nameof(node.statements));

            foreach (var statement in node.statements)
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
            EnsureNotNull(node, node.name, nameof(node.name));

            var @interface = new InterfaceInfo()
            {
                Name = node.name.escapedText,
            };

            if (node.heritageClauses != null)
            {
                foreach (var heritageClause in node.heritageClauses)
                {
                    if (heritageClause.types == null)
                        continue;

                    foreach (var type in heritageClause.types)
                    {
                        EnsureKind(type, SyntaxNodeKind.ExpressionWithTypeArguments);
                        EnsureNotNull(type, type.expression, nameof(type.expression));
                        @interface.Extends.Add(type.expression.escapedText);
                    }
                }
            }

            if (node.members != null)
            {
                foreach (var member in node.members)
                {
                    switch (member.kind)
                    {
                        case SyntaxNodeKind.PropertySignature:
                            @interface.Properties.Add(ParsePropertySignature(context, member));
                            break;

                        //case SyntaxNodeKind.Me:
                        //    @interface.Properties.Add(ParsePropertySignature(context, member));
                        //    break;
                    }
                }
            }

            context.Interfaces.Add(@interface);
        }

        private static PropertyInfo ParsePropertySignature(Context context, SyntaxNode node)
        {
            EnsureNotNull(node, node.name, nameof(node.name));
            EnsureNotNull(node, node.type, nameof(node.type));

            return new PropertyInfo()
            {
                Name = node.name.escapedText,
                Type = ParseType(context, node.type)
            };
        }

        private static string ParseType(Context context, SyntaxNode node)
        {
            switch (node.kind)
            {
                case SyntaxNodeKind.BooleanKeyword:
                    return "bool";

                case SyntaxNodeKind.NumberKeyword:
                    return "int";

                case SyntaxNodeKind.StringKeyword:
                    return "string";

                default:
                    return "?";
            }
        }
    }
}
