using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace WasmWrangler.BindingGenerator
{
    public static class SyntaxNodeExtensions
    {
        public static bool IsDocumentationTrivia(this SyntaxTrivia trivia) =>
            trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
            trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia);

        public static bool HasDocumentation(this SyntaxNode node) =>
            node.HasLeadingTrivia &&
            node.GetLeadingTrivia().Any(x => x.IsDocumentationTrivia());

        public static string GetDocumentation(this SyntaxNode node, string indent)
        {
            var docs = node.GetLeadingTrivia().ToString().Split(Environment.NewLine);
            var output = new List<string>();
            output.Add($"/// <summary>");

            bool closeSummary = true;

            foreach (var line in docs)
            {
                var trimmed = line.Trim();

                if (trimmed.Length == 0)
                    continue;

                int i = 0;
                while (i < trimmed.Length && (trimmed[i] == '/' || trimmed[i] == '*' || trimmed[i] == ('\\')))
                    i++;

                if (i >= trimmed.Length)
                    continue;

                trimmed = trimmed.Substring(i).Trim();

                if (trimmed.Length == 0 )
                    continue;

                if (trimmed.StartsWith("@param "))
                {
                    output.Add($"{indent}/// </summary>");
                    closeSummary = false;

                    trimmed = trimmed.Substring("@param ".Length);
                    var paramName = "";

                    i = 0;
                    while (i < trimmed.Length && trimmed[i] != ' ')
                        i++;

                    if (i < trimmed.Length && trimmed[i] == ' ')
                    {
                        paramName = trimmed.Substring(0, i);
                        trimmed = trimmed.Substring(i).Trim();
                        trimmed = $"<param name=\"{paramName}\">{trimmed}</param>";
                    }
                }

                output.Add($"{indent}/// {trimmed}");
            }

            if (closeSummary)
                output.Add($"{indent}/// </summary>");

            return string.Join(Environment.NewLine, output);
        }
    }
}
