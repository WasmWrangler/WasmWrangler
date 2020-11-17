﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace WasmWrangler.BindingGenerator
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Please provide at least 1 argument.");
                return 1;
            }

            var inputFile = Path.GetFullPath(args[0]);
            var outputFile = Path.Combine(Path.GetDirectoryName(inputFile)!, Path.GetFileNameWithoutExtension(inputFile) + ".g.cs");

            Console.WriteLine($"{inputFile} => {outputFile}");

            var binding = JsonSerializer.Deserialize<WasmWranglerBinding>(File.ReadAllText(inputFile));

            if (binding == null)
                return 1;

            var sb = new StringBuilder(1024);

            sb.AppendLine("// <auto-generated />");
            sb.AppendLine("#nullable enable");
            sb.AppendLine("using WebAssembly;");
            sb.AppendLine();
            sb.AppendLine("namespace WasmWrangler");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static partial class JS");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic static partial class {binding.Name}");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tprivate static JSObject? __js;");
            sb.AppendLine();
            sb.AppendLine("\t\t\tprivate static JSObject _js");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tget");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tif (__js == null)");
            sb.AppendLine($"\t\t\t\t\t\t__js = (JSObject)Runtime.GetGlobalObject(nameof({binding.Name}));");
            sb.AppendLine();
            sb.AppendLine("\t\t\t\t\treturn __js;");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine();

            foreach (var method in binding.Methods)
                GenerateMethodBinding(sb, method);

            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            File.WriteAllText(outputFile, sb.ToString());

            return 0;
        }

        private static void GenerateMethodBinding(StringBuilder sb, WasmWranglerMethodBinding method)
        {
            sb.Append($"\t\t\tpublic static {method.ReturnType} {method.Name}(");

            for (int i = 0; i < method.Args.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                if (method.Args[i].Params)
                    sb.Append("params ");

                sb.Append($"{method.Args[i].Type} {method.Args[i].Name}");
            }

            sb.AppendLine(")");
            sb.AppendLine("\t\t\t{");

            sb.Append("\t\t\t\t");

            if (method.ReturnType != "void")
                sb.Append($"return ({method.ReturnType})");

            sb.Append($"_js.Invoke(nameof({method.Name})");

            for (int i = 0; i < method.Args.Length; i++)
                sb.Append($", {method.Args[i].Name}");

            sb.AppendLine(");");
            sb.AppendLine("\t\t\t}");

            sb.AppendLine();
        }
    }
}