using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace WasmWrangler.BindingGenerator
{
    public static class DTSToJson
    {
        public static SyntaxNode Convert(string inputFile)
        {
            var outputFileName = inputFile + ".json";
            string output = "";

            if (!File.Exists(outputFileName))
            {
                using var process = new Process();
                process.StartInfo.FileName = "node";
                process.StartInfo.Arguments = $"dts-to-json.js {inputFile}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();

                output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                File.WriteAllText(outputFileName, output);
            }
            else
            {
                output = File.ReadAllText(outputFileName);
            }

            var root = JsonSerializer.Deserialize<SyntaxNode>(output);

            if (root == null)
                throw new InvalidOperationException("Failed to parse json");

            return root;
        }
    }
}
