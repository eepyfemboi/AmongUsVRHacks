using HarmonyLib;
using MelonLoader;
using UnityEngine;
using Il2CppSG.Airlock.Network;
using Il2CppSG.Airlock;
using Il2CppFusion;

namespace AmongUsHacks.Features
{
    public static class DeathLogger
    {
        private static bool _enabled = false;
        public static bool Enabled => _enabled;

        // Call from the InputHandler when D is pressed
        public static void Toggle()
        {
            _enabled = !_enabled;
            MelonLogger.Msg($"DeathLogger is now {(_enabled ? "ON" : "OFF")}");
        }
    }

    [HarmonyPatch(typeof(NetworkedKillBehaviour), nameof(NetworkedKillBehaviour.RPC_KillerVFX))]
    public class DeathLoggerPatch
    {
        public static void Prefix(PlayerRef killer)
        {
            if (!DeathLogger.Enabled)
                return;

            var go = GameObject.Find($"PlayerState ({killer.PlayerId})");
            if (go == null) return;

            var name = go.GetComponent<PlayerState>().NetworkName.Value;
            MelonLogger.Msg($"{name} just killed someone!");
        }
    }

    [HarmonyPatch(typeof(SpawnManager), nameof(SpawnManager.RPC_SpawnBodyByPlayerId))]
    public class SpawnBodyPatch
    {
        public static void Prefix(PlayerRef id, NetworkRigidbodyObsolete rb)
        {
            if (!DeathLogger.Enabled)
                return;

            var go = GameObject.Find($"PlayerState ({id.PlayerId})");
            if (go == null) return;

            var name = go.GetComponent<PlayerState>().NetworkName.Value;
            MelonLogger.Msg($"{name} has been killed!");
        }
    }
}
