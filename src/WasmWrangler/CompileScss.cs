using System;
using System.IO;
using Microsoft.Build.Framework;
using SharpScss;

namespace WasmWrangler
{
    public class CompileScss : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string[] InputFiles { get; set; } = Array.Empty<string>();

        [Required]
        public string OutputPath { get; set; } = "";

        public override bool Execute()
        {
            var options = new ScssOptions()
            {
                GenerateSourceMap = true
            };

            foreach (var file in InputFiles)
            {
                try
                {
                    options.InputFile = file;
                    options.OutputFile = Path.Combine(OutputPath, Path.GetFileNameWithoutExtension(file) + ".css");
                    var result = Scss.ConvertFileToCss(file, options);
                    File.WriteAllText(options.OutputFile, result.Css);
                    File.WriteAllText(options.OutputFile + ".map", result.SourceMap);
                }
                catch (Exception ex)
                {
                    Log.LogError($"{nameof(CompileScss)} failed while trying to compile \"{file}\":");
                    Log.LogErrorFromException(ex);
                    return false;
                }
            }

            return true;
        }
    }
}
