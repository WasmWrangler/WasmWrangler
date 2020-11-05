using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;


namespace WasmWranglerDotNet
{
    public class GetMonoWasmSdk : Microsoft.Build.Utilities.Task
    {
        public const string SdkUrl = "https://xamjenkinsartifact.blob.core.windows.net/test-mono-mainline-wasm/5471/ubuntu-1804-amd64/sdks/wasm/mono-wasm-8513b8bbc40.zip";

        [Output]
        public string SdkPath { get; set; }

        public override bool Execute()
        {
            try
            {
                SdkPath = GetSdkPath();
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine (ex);
                Log.LogErrorFromException(ex);
                return false;
            }
        }

        // Taken from Ooui.Wasm: https://github.com/praeclarum/Ooui/blob/master/Ooui.Wasm.Build.Tasks/BuildDistTask.cs#L56
        private string GetSdkPath()
        {
            var sdkName = Path.GetFileNameWithoutExtension(new Uri(SdkUrl).AbsolutePath.Replace('/', Path.DirectorySeparatorChar));
            Log.LogMessage(MessageImportance.High, "SDK: " + sdkName);
            string tmpDir = Path.GetTempPath();
            var sdkPath = Path.Combine(tmpDir, sdkName);
            Log.LogMessage(MessageImportance.High, "SDK Path: " + sdkPath);

            if (Directory.Exists(sdkPath))
                return sdkPath;

            var client = new WebClient();
            var zipPath = sdkPath + ".zip";
            Log.LogMessage(MessageImportance.High, $"Downloading {sdkName} to {zipPath}");
            if (File.Exists(zipPath))
                File.Delete(zipPath);
            client.DownloadFile(SdkUrl, zipPath);

            var sdkTempPath = Path.Combine(tmpDir, Guid.NewGuid().ToString());
            ZipFile.ExtractToDirectory(zipPath, sdkTempPath);
            if (Directory.Exists(sdkPath))
                Directory.Delete(sdkPath, true);
            Directory.Move(sdkTempPath, sdkPath);
            Log.LogMessage(MessageImportance.High, $"Extracted {sdkName} to {sdkPath}");

            return sdkPath;
        }
    }
}
