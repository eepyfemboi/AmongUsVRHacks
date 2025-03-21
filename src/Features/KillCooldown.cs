using UnityEngine;
using MelonLoader;
using AmongUsHacks.Data;
using AmongUsHacks.Utils;

namespace AmongUsHacks.Features
{
    public static class KillCooldown
    {
        public static bool active = false;

        public static void Toggle()
        {
            MelonLogger.Msg($"Toggling kill cooldown to {!active}");
            active = !active;
        }

        public static void UpdateCooldown()
        {
            Helpers.RefreshKillManager();
            if (active)
            {
                if (Globals.killManager != null && Globals.killManager._killCooldown > 1)
                {
                    Globals.killManager._killCooldown = 0.1f;
                    MelonLogger.Msg("Kill cooldown updated.");
                }
            }
        }
    }
}
