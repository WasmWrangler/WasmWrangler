using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WasmWrangler.Build
{
    public static class Utils
    {
        public static List<string> GetReferencedAssemblies(string sdkPath, string assemblyPath)
        {
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFile(assemblyPath);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Failed to load assembly \"{assemblyPath}\".");
            }

            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            if (assemblyDirectory == null)
                throw new InvalidOperationException($"Assembly directory is null.");

            var referencedAssemblies = new List<string>();

            ItereateReferenceAssemblies(sdkPath, assembly, assemblyDirectory, referencedAssemblies);

            // Not sure why this isn't showing up in the referenced assemblies but we need it to be included.
            if (!referencedAssemblies.Contains("WasmWrangler.dll"))
                referencedAssemblies.Add("WasmWrangler.dll");

            if (!referencedAssemblies.Contains("WebAssembly.Bindings.dll"))
                referencedAssemblies.Add("WebAssembly.Bindings.dll");

            return referencedAssemblies;

            static void ItereateReferenceAssemblies(string sdkPath, Assembly assembly, string assemblyDirectory, List<string> referencedAssemblies)
            {
                foreach (var referencedAssemblyName in assembly.GetReferencedAssemblies())
                {
                    if (referencedAssemblyName.Name == null)
                        break;

                    var referencedAssemblyFileName = referencedAssemblyName.Name + ".dll";

                    if (referencedAssemblies.Contains(referencedAssemblyFileName))
                        break;

                    Assembly? referencedAssembly = null;

                    var localAssemblyPath = Path.Combine(assemblyDirectory, referencedAssemblyFileName);
                    if (File.Exists(localAssemblyPath))
                    {
                        referencedAssembly = Assembly.LoadFile(localAssemblyPath);
                    }
                    else
                    {
                        var wasmBclPath = Path.Combine(sdkPath, "wasm-bcl", "wasm");

                        var wasmAssemblyPath = Path.Combine(wasmBclPath, referencedAssemblyFileName);
                        if (File.Exists(wasmAssemblyPath))
                        {
                            referencedAssembly = Assembly.LoadFile(wasmAssemblyPath);
                        }
                        else
                        {
                            var wasmFacadePath = Path.Combine(wasmBclPath, "Facades", referencedAssemblyFileName);
                            if (File.Exists(wasmFacadePath))
                            {
                                referencedAssembly = Assembly.LoadFile(wasmFacadePath);
                            }
                        }
                    }

                    if (referencedAssembly == null)
                        throw new InvalidOperationException($"Unable to load assembly \"{referencedAssemblyName.Name}\".");

                    referencedAssemblies.Add(referencedAssemblyFileName);

                    ItereateReferenceAssemblies(sdkPath, referencedAssembly, assemblyDirectory, referencedAssemblies);
                }
            }
        }

        public static void CopyFileIfNewer(string source, string destination)
        {
            // TODO(zac): Really implement CopyFileIfNewer
            File.Copy(source, destination, true);
        }
    }
}
