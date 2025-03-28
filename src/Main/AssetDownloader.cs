using MelonLoader;
using AmongUsHacks.Main;
using AmongUsHacks.Data;
using AmongUsHacks.Utils;
using System.Threading.Tasks;
using AmongUsHacks.Features;
using UnityEngine;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmongUsHacks.Main
{
    public class AssetDownloader
    {
        private static readonly string bundleFileName = "AdditionalAssets";
        private static readonly string bundlePath = Path.Combine("Mods", "eepyfemboi", bundleFileName);
        private static readonly string remoteVersionUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/version";
        private static readonly string remoteBundleUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/latest/AdditionalAssets";
        public bool isBundleUpdated = false;


        public override void OnApplicationStart()
        {
            //_ = CheckAndUpdateBundle();
        }

        private async Task CheckAndUpdateBundle()
        {
            try
            {
                string remoteVersion = await GetRemoteVersion();
                MelonLogger.Msg($"Remote bundle version: {remoteVersion}");

                string localVersion = GetLocalBundleVersion();
                MelonLogger.Msg(localVersion != null ? $"Local bundle version: {localVersion}" : "Local bundle not found or unreadable.");

                if (localVersion == null || localVersion != remoteVersion)
                {
                    MelonLogger.Msg("Downloading updated bundle...");
                    await DownloadRemoteBundle();
                    MelonLogger.Msg("Bundle updated successfully.");
                }
                else
                {
                    MelonLogger.Msg("Bundle is up to date.");
                }
                isBundleUpdated = true;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Bundle update failed: {ex.Message}");
            }
        }

        private async Task<string> GetRemoteVersion()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
                return (await client.GetStringAsync(remoteVersionUrl)).Trim();
            }
        }

        private string GetLocalBundleVersion()
        {
            if (!File.Exists(bundlePath))
                return null;

            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null)
            {
                MelonLogger.Warning("Failed to load local bundle.");
                return null;
            }

            TextAsset versionAsset = bundle.LoadAsset<TextAsset>("version.txt");
            string version = versionAsset?.text.Trim();

            bundle.Unload(unloadAllLoadedObjects: false);
            return version;
        }

        private async Task DownloadRemoteBundle()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
                byte[] bundleData = await client.GetByteArrayAsync(remoteBundleUrl);
                Directory.CreateDirectory(Path.GetDirectoryName(bundlePath));
                await File.WriteAllBytesAsync(bundlePath, bundleData);
            }
        }
    }
}