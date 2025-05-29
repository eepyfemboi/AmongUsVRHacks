using System;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using MelonLoader;
using AmongUsHacks.Data;

namespace AmongUsHacks.Main
{
    public static class Updater
    {
        private static readonly string VersionUrl = "https://sleepie.dev/amongusvr/mod/version";
        private static readonly string DownloadUrl = "https://sleepie.dev/amongusvr/mod/latest/AmongUsHacks.dll";
        private static readonly string ModFileName = "AmongUsHacks.dll";
        private static readonly string ModFolderPath = "Mods";
        private static readonly string CurrentVersion = "2.0.3";

        public static async Task CheckForUpdates()
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);

                string latestVersion = (await client.GetStringAsync(VersionUrl)).Trim();

                if (latestVersion != CurrentVersion)
                {
                    MelonLogger.Msg($"New version available: {latestVersion} (Current: {CurrentVersion})");
                    await DownloadAndUpdate();
                }
                else
                {
                    MelonLogger.Msg("You are running the latest version.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Update check failed: {ex.Message}");
            }
        }

        private static async Task DownloadAndUpdate()
        {
            try
            {
                string modFilePath = Path.Combine(ModFolderPath, ModFileName);
                string tempFilePath = modFilePath + ".new";
                string batchFilePath = Path.Combine(ModFolderPath, "UpdateMod.bat");

                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Config.UserAgent);
                byte[] data = await client.GetByteArrayAsync(DownloadUrl);
                await File.WriteAllBytesAsync(tempFilePath, data);

                int au3d_non_vr_game_id = 3168600;
                int au3d_vr_game_id = 1849900;

                int game_id_to_launch;

                if (Globals.isVR)
                {
                    game_id_to_launch = au3d_vr_game_id;
                } else
                {
                    game_id_to_launch = au3d_non_vr_game_id;
                }

                    File.WriteAllText(batchFilePath, $@"
@echo off
:retry
timeout /t 5 /nobreak >nul
del ""{modFilePath}.old""
move ""{modFilePath}"" ""{modFilePath}.old""
move ""{tempFilePath}"" ""{modFilePath}""
timeout /t 1 /nobreak >nul
start steam://rungameid/{game_id_to_launch}
del ""{batchFilePath}""
exit
"); // i switched from  start """" ""{Environment.ProcessPath}""  to using the steam game launch thingie bcuz i think it should work better (at least on my machine), but lmk if it breaks

                RestartGame(batchFilePath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Update failed: {ex.Message}");
            }
        }

        private static void RestartGame(string batchFilePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = batchFilePath,
                    UseShellExecute = true,
                    CreateNoWindow = true
                });

                MelonLogger.Msg("Game restarting...");
                System.Environment.Exit(0); // im using the system exit bcuz the unity exit was delaying and causing the batch script to fail.
                //UnityEngine.Application.Quit();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to restart: {ex.Message}");
            }
        }
    }
}
