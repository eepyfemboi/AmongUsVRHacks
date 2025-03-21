using UnityEngine;
using MelonLoader;
using AmongUsHacks.Main;
using Il2CppSG.Airlock.XR;

namespace AmongUsHacks.Features
{
    public static class Speed
    {
        public static bool enabled = false;

        public static void Toggle()
        {
            MelonLogger.Msg($"Toggling speed increase to {!enabled}");
            SetPlayerSpeed(enabled ? 6.5f : Config.SpeedSetting);
            enabled = !enabled;
            MelonLogger.Msg($"Toggled speed increase to {enabled}");
        }

        private static void SetPlayerSpeed(float speed)
        {
            int modifiedCount = 0;
            foreach (XRRig rig in GameObject.FindObjectsOfType<XRRig>())
            {
                rig._speed = speed;
                modifiedCount++;
                MelonLogger.Msg($"Modified {rig.gameObject.name}: _speed set to {speed}");
            }

            MelonLogger.Msg($"Finished updating {modifiedCount} XRRig instances.");
        }
    }
}
