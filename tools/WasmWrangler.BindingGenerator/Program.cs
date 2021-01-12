using System;
using System.IO;
using System.Text.Json;

namespace WasmWrangler.BindingGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = JsonSerializer.Deserialize<SyntaxNode>(File.ReadAllText(Path.GetFullPath(args[0])));
        }
    }
}
