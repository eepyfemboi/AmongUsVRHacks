/*using MelonLoader;
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
    public static class AssetDownloader
    {
        private static readonly string bundleFileName = "AdditionalAssets";
        private static readonly string bundlePath = Path.Combine("Mods", "eepyfemboi", bundleFileName);
        private static readonly string remoteVersionUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/version";
        private static readonly string remoteBundleUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/latest/AdditionalAssets";
        public static bool isBundleUpdated = false;


        /*public override void OnApplicationStart()
        {
            //_ = CheckAndUpdateBundle();
        }* /

public static async Task CheckAndUpdateBundle()
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

        private static async Task<string> GetRemoteVersion()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
                return (await client.GetStringAsync(remoteVersionUrl)).Trim();
            }
        }

        private static string GetLocalBundleVersion()
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

        private static async Task DownloadRemoteBundle()
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
}*/

/*using MelonLoader;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;


namespace AmongUsHacks.Main
{
    public static class AssetDownloader
    {
        private static readonly string bundleFileName = "AdditionalAssets";
        private static readonly string bundlePath = Path.Combine("Mods", "eepyfemboi", bundleFileName);
        private static readonly string remoteVersionUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/version";
        private static readonly string remoteBundleUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/latest/AdditionalAssets";
        public static bool isBundleUpdated = false;

        private static TaskCompletionSource<string> versionTcs;

        public static async Task CheckAndUpdateBundle()
        {
            try
            {
                string remoteVersion = await GetRemoteVersion();
                MelonLogger.Msg($"Remote bundle version: {remoteVersion}");

                string localVersion = await GetLocalBundleVersion();
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

        private static async Task<string> GetRemoteVersion()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
                return (await client.GetStringAsync(remoteVersionUrl)).Trim();
            }
        }

        private static async Task<string> GetLocalBundleVersion()
        {
            if (!File.Exists(bundlePath))
                return null;

            byte[] bundleBytes = await File.ReadAllBytesAsync(bundlePath);
            if (bundleBytes == null || bundleBytes.Length == 0)
                return null;

            versionTcs = new TaskCompletionSource<string>();
            MelonCoroutines.Start(LoadBundleVersionFromBytes(bundleBytes));
            return await versionTcs.Task;
        }

        private static IEnumerator LoadBundleVersionFromBytes(byte[] bundleBytes)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "temp_asset_bundle");

            try
            {
                File.WriteAllBytes(tempPath, bundleBytes);
                AssetBundle bundle = AssetBundle.LoadFromFile(tempPath);
                if (bundle == null)
                {
                    MelonLogger.Warning("Failed to load local bundle.");
                    versionTcs.SetResult(null);
                    yield break;
                }

                TextAsset versionAsset = bundle.LoadAsset<TextAsset>("version.txt");
                string version = versionAsset?.text?.Trim();
                bundle.Unload(false);
                File.Delete(tempPath);

                versionTcs.SetResult(version);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Error during bundle version extraction: {ex}");
                versionTcs.SetResult(null);
            }

            yield return null;
        }

        private static async Task DownloadRemoteBundle()
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
}*/

using MelonLoader;
using System;
using System.IO;
using System.Net;
using UnityEngine;

namespace AmongUsHacks.Main
{
    public static class AssetDownloader
    {
        private static readonly string bundleFileName = "AdditionalAssets";
        private static readonly string bundlePath = Path.Combine("Mods", "eepyfemboi", bundleFileName);
        private static readonly string remoteVersionUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/version";
        private static readonly string remoteBundleUrl = "https://sleepie.dev/amongusvr/mod/additional_assets/latest/AdditionalAssets";

        public static bool isBundleUpdated = false;

        public static void CheckAndUpdateBundle()
        {
            try
            {
                string remoteVersion = GetRemoteVersion();
                MelonLogger.Msg($"Remote bundle version: {remoteVersion}");

                string localVersion = GetLocalBundleVersion();
                MelonLogger.Msg(localVersion != null ? $"Local bundle version: {localVersion}" : "Local bundle not found or unreadable.");

                if (localVersion == null || localVersion != remoteVersion)
                {
                    MelonLogger.Msg("Downloading updated bundle...");
                    DownloadRemoteBundle();
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
                MelonLogger.Error($"Bundle update failed: {ex}");
            }
        }

        private static string GetRemoteVersion()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", Config.UserAgent);
                return client.DownloadString(remoteVersionUrl).Trim();
            }
        }

        private static string GetLocalBundleVersion()
        {
            if (!File.Exists(bundlePath))
                return null;

            try
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle == null)
                {
                    MelonLogger.Warning("Failed to load AssetBundle.");
                    return null;
                }

                TextAsset versionAsset = bundle.LoadAsset<TextAsset>("version.txt");
                string version = versionAsset?.text?.Trim();

                bundle.Unload(false);
                return version;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Error reading local bundle: {ex}");
                return null;
            }
        }

        private static void DownloadRemoteBundle()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", Config.UserAgent);
                byte[] bundleData = client.DownloadData(remoteBundleUrl);

                Directory.CreateDirectory(Path.GetDirectoryName(bundlePath));
                File.WriteAllBytes(bundlePath, bundleData);
            }
        }
    }
}

