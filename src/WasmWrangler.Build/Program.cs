using System;
using System.IO;
using System.IO.Compression;
using System.Net;

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
    }
}
