using System.IO;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using MelonLoader.Utils;
using AmongUsHacks.Main;

namespace AmongUsHacks.Data
{
    public static class BlacklistManager
    {
        private static readonly string filePath = Path.Combine(MelonEnvironment.GameRootDirectory, "blacklist.txt");
        public static List<string> blacklist = new();
        private static bool blacklistUpdated = false;

        public static void LoadBlacklist()
        {
            if (!blacklistUpdated && Config.DoRemoteBlacklistUpdate)
            {
                MelonLogger.Msg("Attempting to fetch updated blacklist...");
                FetchBlacklist();
            }

            if (File.Exists(Config.BlacklistFilePath))
            {
                blacklist = File.ReadAllLines(Config.BlacklistFilePath)
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line))
                    .ToList();

                MelonLogger.Msg($"Loaded {blacklist.Count} blacklist entries.");
            }
            else
            {
                MelonLogger.Warning($"Blacklist file not found: {Config.BlacklistFilePath}");
            }
        }
        public static void Load()
        {
            if (File.Exists(filePath))
                blacklist = File.ReadAllLines(filePath).Select(line => line.Trim()).Where(line => !string.IsNullOrEmpty(line)).ToList();
            else
                MelonLogger.Warning($"Blacklist file not found: {filePath}");
        }

        public static bool Matches(string objectName)
        {
            return blacklist.Any(phrase => objectName.Contains(phrase));
        }

        private static void FetchBlacklist()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
                    MelonLogger.Msg($"Updating blacklisted objects from url: {Config.RemoteBlacklistURL}");
                    string response = client.GetStringAsync(Config.RemoteBlacklistURL).GetAwaiter().GetResult();
                    File.WriteAllText(Config.BlacklistFilePath, response);
                    MelonLogger.Msg("Finished updating blacklisted objects");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"Failed to fetch blacklisted objects: {ex.Message}");
            }

            blacklistUpdated = true;
        }
    }
}
