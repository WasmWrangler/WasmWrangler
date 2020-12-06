using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

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

            var assembliesJs = string.Join(", ", assembliesToPackage.Select(x => $"'{x}'"));
            File.WriteAllText(Path.Combine(outputDirectory, "package.js"), assembliesJs);

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
    }
}
