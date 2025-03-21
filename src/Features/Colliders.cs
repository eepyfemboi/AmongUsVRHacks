using UnityEngine;
using MelonLoader;
using AmongUsHacks.Data;

namespace AmongUsHacks.Features
{
    public static class Colliders
    {
        public static bool toggled = false;
        private static System.Collections.Generic.List<GameObject> disabledObjectsList = new System.Collections.Generic.List<GameObject>();

        public static void Toggle()
        {
            MelonLogger.Msg($"Toggling colliders to {!toggled}");
            BlacklistManager.Load();

            if (toggled)
                ReEnableDisabledObjects();
            else
                DisableMatchingObjects();

            toggled = !toggled;
            MelonLogger.Msg($"Toggled colliders to {toggled}");
        }

        private static void DisableMatchingObjects()
        {
            int disabledCount = 0;
            disabledObjectsList = new System.Collections.Generic.List<GameObject>();

            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (BlacklistManager.blacklist.Any(phrase => obj.name.IndexOf(phrase, System.StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    obj.SetActive(false);
                    disabledObjectsList.Add(obj);
                    disabledCount++;
                    MelonLogger.Msg($"Disabled: {obj.name}");
                }
            }

            MelonLogger.Msg($"Finished disabling {disabledCount} objects.");
        }

        private static void ReEnableDisabledObjects()
        {
            int enabledCount = 0;
            try
            {
                foreach (GameObject obj in disabledObjectsList)
                {
                    try
                    {
                        obj.SetActive(true);
                        enabledCount++;
                        MelonLogger.Msg($"Re-Enabled: {obj.name}");
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Msg($"Failed to re-enable object: {obj.name}");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg(ex);
            }

            MelonLogger.Msg($"Finished re-enabling {enabledCount} objects");
        }
    }
}
