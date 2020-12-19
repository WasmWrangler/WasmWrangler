using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using NUglify;
using NUglify.JavaScript;

namespace WasmWrangler.Build
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
                case nameof(DownloadMonoWasmSDK):
                    if (args.Length < 4)
                    {
                        Console.Error.WriteLine($"Please provide 3 arguments for {nameof(DownloadMonoWasmSDK)}.");
                        return 1;
                    }

                    if (!DownloadMonoWasmSDK(args[1], args[2], args[3]))
                        return 1;

                    break;

                case nameof(PackageAssembly):
                    if (args.Length < 5)
                    {
                        Console.Error.WriteLine($"Please provide 4 arguments for {nameof(PackageAssembly)}.");
                        return 1;
                    }

                    return PackageAssembly(args[1], args[2], args[3], args[4].Equals("true", StringComparison.InvariantCultureIgnoreCase));

                case nameof(MinifyJs):
                    if (args.Length < 3)
                    {
                        Console.Error.WriteLine($"Please provide 2 arguments for {nameof(MinifyJs)}.");
                        return 1;
                    }

                    return MinifyJs(args[1], args[2]);
            }

            return 0;
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

                if (Directory.Exists(sdkPath))
                    Directory.Delete(sdkPath, true);

                ZipFile.ExtractToDirectory(zipPath, sdkPath);
                Console.WriteLine($"Extracted {sdkName} to {sdkPath}");

                File.Delete(zipPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
        }

        private static int PackageAssembly(string sdkPath, string assemblyPath, string outputDirectory, bool debug)
        {
            List<string> referencedAssemblies;

            try
            {
                referencedAssemblies = Utils.GetReferencedAssemblies(sdkPath, assemblyPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed while determining referenced assemblies:");
                Console.Error.WriteLine(ex);
                return 1;
            }

            // NOTE: After this point no debugging information is available due to loading of assemblies.

            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            if (assemblyDirectory == null)
            {
                // Not sure how we would get here.
                Console.Error.WriteLine($"Assembly directory is null.");
                return 1;
            }

            var assembliesToPackage = Enumerable.Repeat(Path.GetFileName(assemblyPath), 1)
                .Concat(referencedAssemblies)
                .ToArray();

            foreach (var assembly in assembliesToPackage)
            {
                if (!CopyAssembly(sdkPath, assemblyDirectory, assembly, outputDirectory, debug))
                {
                    Console.Error.WriteLine($"Failed to copy assembly \"{assembly}\".");
                    //return 2;
                }
            }

            // Path.GetFileName will give us the last directory, usually "managed"
            var packageDirectory = Path.GetFileName(outputDirectory);

            var enableDebugging = debug ? "1" : "0";
            
            var packageJs = $"var config={{vfs_prefix:\"{packageDirectory}\",deploy_prefix:\"{packageDirectory}\",enable_debugging:{enableDebugging},file_list:[";
            packageJs += string.Join(",", assembliesToPackage.Select(x => $"'{x}'"));
            packageJs += "]};";

            File.WriteAllText(Path.Combine(outputDirectory, "package.js"), packageJs);

            return 0;

            static bool CopyAssembly(string sdkPath, string assemblyDirectory, string assemblyFileName, string outputDirectroy, bool debug)
            {
                var assemblyPath = Path.Combine(assemblyDirectory, assemblyFileName);

                if (!File.Exists(assemblyPath))
                {
                    var wasmBclPath = Path.Combine(sdkPath, "wasm-bcl", "wasm");

                    var wasmAssemblyPath = Path.Combine(wasmBclPath, assemblyFileName);
                    if (File.Exists(wasmAssemblyPath))
                    {
                        assemblyPath = wasmAssemblyPath;
                    }
                    else
                    {
                        var wasmFacadePath = Path.Combine(wasmBclPath, "Facades", assemblyFileName);
                        if (File.Exists(wasmFacadePath))
                        {
                            assemblyPath = wasmFacadePath;
                        }
                    }
                }

                if (File.Exists(assemblyPath))
                {
                    Utils.CopyFileIfNewer(assemblyPath, Path.Combine(outputDirectroy, Path.GetFileName(assemblyPath)));
                }
                else
                {
                    return false;
                }

                return true;
            }
        }

        private static int MinifyJs(string inputFileName, string outputFileName)
        {
            try
            {
                string input = File.ReadAllText(inputFileName);
                string output = JsMin.Minify(input);
                File.WriteAllText(outputFileName, output);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while uglifying \"{inputFileName}\":");
                Console.Error.WriteLine(ex);
                return 1;
            }

            return 0;
        }
    }
}
