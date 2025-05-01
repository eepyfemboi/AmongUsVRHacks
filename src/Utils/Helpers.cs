using MelonLoader;
using UnityEngine;
using Il2CppFusion;
using Il2CppSG.Airlock.Network;
using Exception = System.Exception;
using Il2Cpp;
using AmongUsHacks.Data;
using Il2CppSG.Airlock.UI;
using Il2CppSG.Airlock;


namespace AmongUsHacks.Utils
{
    public static class Helpers
    {
        public static void RefreshKillManager()
        {
            if (Globals.killManager == null)
            {
                Globals.killManager = GameObject.FindObjectOfType<NetworkedKillBehaviour>();
            }
        }

        public static GameObject? GetPlayerGhostObject(GameObject player)
        {
            foreach (Transform transform in player.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject.name.Contains("ghost", System.StringComparison.OrdinalIgnoreCase))
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        public static void SetImposterNametag(NetworkedLocomotionPlayer player, bool isImposter)
        {
            UINameTag nameTag = player._nameTag;
            nameTag.IsImpostorText = isImposter;
        }

        public static bool HasSpeakerGameObject1(GameObject obj)
        {
            foreach (Transform i in obj.transform)
            {
                if (i.gameObject.name == "Speaker") return true;
                if (HasSpeakerGameObject1(i.gameObject)) return true;
            }
            return false;
        }

        public static bool HasSpeakerGameObject(GameObject obj)
        {
            foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
            {
                if (child.gameObject.name == "Speaker") return true;
            }
            return false;
        }

        public static System.Collections.Generic.List<NetworkedLocomotionPlayer> GetRealPlayers()
        {
            System.Collections.Generic.List<NetworkedLocomotionPlayer> realPlayers = new System.Collections.Generic.List<NetworkedLocomotionPlayer>();

            foreach (NetworkedLocomotionPlayer player in GetNetworkedLocomotionPlayers())
            {
                try
                {
                    if (player._cachedPlayerID < 0)
                    {
                        continue;
                    }
                    realPlayers.Add(player);
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg(ex);
                }
            }
            return realPlayers;
        }

        public static int GetSelfPlayerID()
        {
            int playerId = -1;

            System.Collections.Generic.List<NetworkedLocomotionPlayer> realPlayers = GetRealPlayers();

            foreach (NetworkedLocomotionPlayer player in realPlayers)
            {
                if (HasSpeakerGameObject(player.gameObject))
                {
                    playerId = player._cachedPlayerID;
                    MelonLogger.Msg($"assuming self id is {playerId}");
                }
            }

            return playerId;
        }

        public static PlayerState? GetSelfPlayerState()
        {
            int selfPlayerId = GetSelfPlayerID();
            
            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                if (player.PlayerId == selfPlayerId) { return player; }
            }
            return null;
        }

        public static bool CheckIfCrewmateIsSelf(GameObject obj, int selfPlayerId)
        {
            NetworkedLocomotionPlayer current = obj.GetComponentInParent<NetworkedLocomotionPlayer>();
            if (current._cachedPlayerID == selfPlayerId)
            {
                return true;
            }
            return false;
        }


        public static System.Collections.Generic.List<GameObject> GetChildrenWithRenderers(GameObject parent)
        {
            System.Collections.Generic.List<GameObject> objectsWithRenderers = new System.Collections.Generic.List<GameObject>();

            foreach (Renderer renderer in parent.GetComponentsInChildren<Renderer>(true))
            {
                objectsWithRenderers.Add(renderer.gameObject);
            }

            return objectsWithRenderers;
        }


        public static System.Collections.Generic.List<GameObject> GetCrewmateBodies()
        {
            System.Collections.Generic.List<GameObject> crewmateObjectsList = new System.Collections.Generic.List<GameObject>();

            int selfPlayerId = GetSelfPlayerID();
            MelonLogger.Msg($"ASsuming self player id is {selfPlayerId}");

            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (obj.name.Contains("CrewmatePhysics"))
                {
                    if (!CheckIfCrewmateIsSelf(obj, selfPlayerId))
                    {
                        crewmateObjectsList.Add(obj);
                    }
                }
            }
            return crewmateObjectsList;
        }

        public static System.Collections.Generic.List<NetworkedLocomotionPlayer> GetNetworkedLocomotionPlayers()
        {
            System.Collections.Generic.List<NetworkedLocomotionPlayer> players = new System.Collections.Generic.List<NetworkedLocomotionPlayer>();

            foreach (NetworkedLocomotionPlayer player in GameObject.FindObjectsOfType<NetworkedLocomotionPlayer>())
            {
                players.Add(player);
            }

            return players;
        }

        public static GameObject? GetPlayerUINameTag(GameObject player)
        {
            foreach (Transform transform in player.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject.name.Contains("ui", System.StringComparison.OrdinalIgnoreCase) && transform.gameObject.name.Contains("nametag", System.StringComparison.OrdinalIgnoreCase))
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        public static PlayerRef GetSelfPlayerRef()
        {
            int playerSelfId = GetSelfPlayerID();
            PlayerRef playerRef = (PlayerRef)playerSelfId;
            return playerRef;
        }
    }
}
