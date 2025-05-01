using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;

namespace AmongUsHacks.Main
{
    public static class Config
    {
        private static MelonPreferences_Category settings;

        public static KeyCode CollidersToggleKey { get; private set; }
        public static KeyCode SpeedToggleKey { get; private set; }
        public static KeyCode ImposterToggleKey { get; private set; }
        public static KeyCode WallHackToggleKey { get; private set; }
        public static KeyCode RainbowColorsKey { get; private set; }
        public static KeyCode KillEveryoneActivatorKey { get; private set; }
        public static KeyCode KillCooldownKey { get; private set; }
        public static KeyCode ForceImposterKey { get; private set; }
        public static KeyCode UnignoreGhostsKey { get; private set; }
        public static KeyCode NativeDebugMenuKey { get; private set; }

        public static float SpeedSetting { get; private set; }
        public static int OverlayLayer { get; private set; }
        public static string UserAgent { get; private set; }
        public static string RemoteBlacklistURL { get; private set; }
        public static bool DoRemoteBlacklistUpdate { get; private set; }
        public static string BlacklistFilePath { get; private set; }

        public static void Load()
        {
            settings = MelonPreferences.CreateCategory("AmongUsVRHacksSettings");

            CollidersToggleKey = settings.CreateEntry("ToggleCollidersKeybind", KeyCode.C, "Toggle Colliders Keybind").Value;
            SpeedToggleKey = settings.CreateEntry("ToggleSpeedKeybind", KeyCode.S, "Toggle Speed Keybind").Value;
            ImposterToggleKey = settings.CreateEntry("ImposterToggleKey", KeyCode.I, "Toggle Show Imposters Keybind").Value;
            WallHackToggleKey = settings.CreateEntry("WallHackToggleKey", KeyCode.W, "Toggle Wallhacks Keybind").Value;
            RainbowColorsKey = settings.CreateEntry("RainbowColorsKey", KeyCode.R, "Toggle Rainbow Colors Keybind").Value;
            KillEveryoneActivatorKey = settings.CreateEntry("KillAllActivatorKey", KeyCode.DownArrow, "Toggle Kill Everyone Activator Key").Value;
            KillCooldownKey = settings.CreateEntry("KillCooldownKey", KeyCode.K, "Toggle Kill Cooldown Key").Value;
            ForceImposterKey = settings.CreateEntry("ForceImposterKey", KeyCode.F, "Force Imposter Key").Value;
            UnignoreGhostsKey = settings.CreateEntry("UnignoreGhostsKey", KeyCode.U, "Toggle Force Unignore Ghosts Key").Value;
            NativeDebugMenuKey = settings.CreateEntry("NativeDebugMenuKey", KeyCode.F3, "Toggle Native Debug Menu Key").Value;

            SpeedSetting = settings.CreateEntry("SpeedSetting", 11f, "Speed Increase Value").Value;
            OverlayLayer = settings.CreateEntry("WallHackOverlayLayer", 7, "WallHack Overlay Layer").Value;

            UserAgent = settings.CreateEntry("DefaultNetworkUserAgent", "SleepysAmongUsVRHacks/1.0 (Windows; MelonLoader)", "The default User Agent to use for network requests (such as updater, rpc, data updates, etc)").Value;

            RemoteBlacklistURL = settings.CreateEntry("RemoteBlacklistURL", "https://sleepie.dev/amongusvr/mod/dynamic_data/blacklisted_object_names.txt", "Remote Object Blacklist URL").Value;
            DoRemoteBlacklistUpdate = settings.CreateEntry("DoRemoteBlacklistUpdate", true, "Do Remote Blacklist Update On Load").Value;

            BlacklistFilePath = Path.Combine(MelonEnvironment.GameRootDirectory, "blacklist.txt");
        }
    }
}
