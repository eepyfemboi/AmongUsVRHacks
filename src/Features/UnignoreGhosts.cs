using System;
using MelonLoader;
using UnityEngine;
using Il2CppFusion;
using Il2CppSG.Airlock.Network;
using Exception = System.Exception;
using Il2CppSG.Airlock;
using AmongUsHacks.Utils;


namespace AmongUsHacks.Features
{
    public static class UnignoreGhosts
    {
        public static bool enabled = false;
        private static int forceUnignoreGhostsFrame = 0;
        private static int forceUnignoreGhostsUpdateFrameInterval = 100;


        public static void DoUpdate()
        {
            if (enabled)
            {
                if (forceUnignoreGhostsFrame > forceUnignoreGhostsUpdateFrameInterval)
                {
                    forceUnignoreGhostsFrame = 0;
                    DoUnignoreGhosts();
                }
                else
                {
                    forceUnignoreGhostsFrame++;
                }
            }
        }

        public static void Toggle()
        {
            enabled = !enabled;
            MelonLogger.Msg($"Toggled force unignore ghosts to {enabled}");
        }

        private static void SwitchPlayerToAliveVoiceChatGroup(GameObject player)
        {
            VoiceChatManager manager = UnityEngine.Object.FindObjectOfType<VoiceChatManager>();
            foreach (VoiceChatActivator vcAct in player.GetComponentsInChildren<VoiceChatActivator>())
            {
                AudioSource audioSource = vcAct.gameObject.GetComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = manager._aliveVoiceChatGroup;
            }
        }

        private static void UnignorePlayer(NetworkedLocomotionPlayer player)
        {
            GameObject? playerGhostObj = Helpers.GetPlayerGhostObject(player.gameObject);
            if (playerGhostObj != null)
            {
                foreach (SkinnedMeshRenderer renderer in playerGhostObj.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    renderer.enabled = true;
                }
            }
            GameObject? nameTag = Helpers.GetPlayerUINameTag(player.gameObject);
            if (nameTag != null)
            {
                nameTag.SetActive(true);
            }
            SwitchPlayerToAliveVoiceChatGroup(player.gameObject);
        }

        private static void DoUnignoreGhosts()
        {
            System.Collections.Generic.List<NetworkedLocomotionPlayer> players = Helpers.GetNetworkedLocomotionPlayers();
            int selfPlayerId = Helpers.GetSelfPlayerID();
            foreach (NetworkedLocomotionPlayer player in Helpers.GetRealPlayers())
            {
                if (player._cachedPlayerID != selfPlayerId)
                {
                    UnignorePlayer(player);
                    MelonLogger.Msg($"Unignoring player: {player._nameTag._storedName}");
                }
            }
        }
    }
}