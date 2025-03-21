using UnityEngine;
using MelonLoader;
using Il2CppSG.Airlock.Roles;
using Il2CppSG.Airlock.Network;
using AmongUsHacks.Utils;

namespace AmongUsHacks.Features
{
    public static class ImposterReveal
    {
        public static bool enabled = false;

        public static void Toggle()
        {
            MelonLogger.Msg($"Toggling show imposters to {!enabled}");

            if (enabled)
                HideImposters();
            else
                ShowImposters();

            enabled = !enabled;
            MelonLogger.Msg($"Toggled show imposters to {enabled}");
        }

        private static void HideImposters()
        {
            foreach (NetworkedLocomotionPlayer player in Helpers.GetNetworkedLocomotionPlayers())
            {
                try
                {
                    Helpers.SetImposterNametag(player, false);
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg(ex);
                }
            }
        }

        private static void ShowImposters()
        {
            RoleManager roleManager = GameObject.FindObjectOfType<RoleManager>();

            if (roleManager.gameRoleToPlayerIds.TryGetValue(GameRole.Imposter, out Il2CppSystem.Collections.Generic.List<int> imposterPlayerIDs))
            {
                MelonLogger.Msg($"Raw Dict: {roleManager.gameRoleToPlayerIds.ToString()}  Gathered Player IDs: {imposterPlayerIDs}");
                foreach (NetworkedLocomotionPlayer player in Helpers.GetNetworkedLocomotionPlayers())
                {
                    try
                    {
                        MelonLogger.Msg($"Current Player ID: {player._cachedPlayerID}");
                        if (imposterPlayerIDs.Contains(player._cachedPlayerID))
                        {
                            MelonLogger.Msg($"Setting player ID {player._cachedPlayerID} name {player._nameTag._storedName} as imposter");
                            Helpers.SetImposterNametag(player, true);
                            MelonLogger.Msg($"Player ID {player._cachedPlayerID} name {player._nameTag._storedName} has been set as imposter");
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Msg(ex);
                    }
                }
            } else
            {
                MelonLogger.Msg("Unable to show imposters.");
            }
        }
    }
}
