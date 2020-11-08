using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using SharpScss;

namespace WasmWrangler
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Please provide a command name.");
                return 1;
            }

            switch (args[0])
            {
                case nameof(CompileScss):
                    if (args.Length < 3)
                    {
                        Console.Error.WriteLine($"Please provide 2 arguments for {nameof(CompileScss)}.");
                        return 1;
                    }

                    if (!CompileScss(args[1], args[2]))
                        return 1;

                    break;

                case nameof(DownloadMonoWasmSDK):
                    if (args.Length < 4)
                    {
                        Console.Error.WriteLine($"Please provide 3 arguments for {nameof(DownloadMonoWasmSDK)}.");
                        return 1;
                    }

                    if (!DownloadMonoWasmSDK(args[1], args[2], args[3]))
                        return 1;

                    break;
            }

            return 0;
        }

        private static bool CompileScss(string inputFile, string outputFile)
        {
            Console.WriteLine($"{nameof(CompileScss)}: {inputFile} => {outputFile}");

            var options = new ScssOptions()
            {
                GenerateSourceMap = true
            };

            try
            {
                options.InputFile = inputFile;
                options.OutputFile = outputFile;
                var result = Scss.ConvertFileToCss(inputFile, options);
                File.WriteAllText(outputFile, result.Css);
                File.WriteAllText(outputFile + ".map", result.SourceMap);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{nameof(CompileScss)} failed while trying to compile \"{inputFile}\":");
                Console.Error.WriteLine(ex);
                return false;
            }

            return true;
        }

        // Taken from Ooui.Wasm: https://github.com/praeclarum/Ooui/blob/master/Ooui.Wasm.Build.Tasks/BuildDistTask.cs#L56
        private static bool DownloadMonoWasmSDK(string sdkUrl, string sdkName, string sdkPath)
        {
            Console.WriteLine(nameof(DownloadMonoWasmSDK));

            try
            {
                if (Directory.Exists(sdkPath))
                    return true;

                var client = new WebClient();
                var zipPath = sdkPath + ".zip";
                Console.WriteLine($"Downloading {sdkName} to {zipPath}");
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
                client.DownloadFile(sdkUrl, zipPath);

                var sdkTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                ZipFile.ExtractToDirectory(zipPath, sdkTempPath);
                if (Directory.Exists(sdkPath))
                    Directory.Delete(sdkPath, true);
                Directory.Move(sdkTempPath, sdkPath);
                Console.WriteLine($"Extracted {sdkName} to {sdkPath}");

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
        }
    }
}
