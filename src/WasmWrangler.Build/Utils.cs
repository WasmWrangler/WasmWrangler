using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WasmWrangler.Build
{
    public static class Utils
    {
        public static List<string> GetReferencedAssemblies(string assemblyPath)
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

            ItereateReferenceAssemblies(assembly, assemblyDirectory, referencedAssemblies);

            return referencedAssemblies;

            static void ItereateReferenceAssemblies(Assembly assembly, string assemblyDirectory, List<string> referencedAssemblies)
            {
                foreach (var referencedAssemblyName in assembly.GetReferencedAssemblies())
                {
                    if (referencedAssemblyName.Name == null || referencedAssemblies.Contains(referencedAssemblyName.Name))
                        break;

                    Assembly referencedAssembly;

                    var localAssemblyPath = Path.Combine(assemblyDirectory, referencedAssemblyName.Name + ".dll");
                    if (File.Exists(localAssemblyPath))
                    {
                        referencedAssembly = Assembly.LoadFile(localAssemblyPath);
                    }
                    else
                    {
                        referencedAssembly = Assembly.Load(referencedAssemblyName);
                    }

                    referencedAssemblies.Add(referencedAssemblyName.Name);

                    ItereateReferenceAssemblies(referencedAssembly, assemblyDirectory, referencedAssemblies);
                }
            }
        }
    }
}
