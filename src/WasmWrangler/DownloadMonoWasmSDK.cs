using Microsoft.Build.Framework;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;


namespace WasmWrangler
{
    public class DownloadMonoWasmSDK : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string SDKUrl { get; set; } = "";

        [Required]
        public string SDKName { get; set; } = "";

        [Required]
        public string SDKPath { get; set; } = "";

        // Taken from Ooui.Wasm: https://github.com/praeclarum/Ooui/blob/master/Ooui.Wasm.Build.Tasks/BuildDistTask.cs#L56
        public override bool Execute()
        {
            try
            {
                if (Directory.Exists(SDKPath))
                    return true;

                var client = new WebClient();
                var zipPath = SDKPath + ".zip";
                Log.LogMessage(MessageImportance.High, $"Downloading {SDKName} to {zipPath}");
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
                client.DownloadFile(SDKUrl, zipPath);

                var sdkTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                ZipFile.ExtractToDirectory(zipPath, sdkTempPath);
                if (Directory.Exists(SDKPath))
                    Directory.Delete(SDKPath, true);
                Directory.Move(sdkTempPath, SDKPath);
                Log.LogMessage(MessageImportance.High, $"Extracted {SDKName} to {SDKPath}");

                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex);
                Log.LogErrorFromException(ex);
                return false;
            }
        }
    }
}
