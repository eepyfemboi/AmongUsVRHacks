using UnityEngine;
using MelonLoader;
using Il2CppSG.Airlock.Network;
using Il2Cpp;
using Il2CppFusion;
using Il2CppSG.Airlock;
using AmongUsHacks.Utils;
using AmongUsHacks.Data;
using AmongUsHacks.Main;
using Input = AmongUsHacks.Main.Input;

namespace AmongUsHacks.Features
{
    public static class KillEveryone
    {
        public static bool killEveryoneActive = false;
        public static bool killEveryoneUseSelf = false;

        public static void TestKillEveryoneMethod1()
        {
            Helpers.RefreshKillManager();

            System.Collections.Generic.List<AirlockPeer> peers = new System.Collections.Generic.List<AirlockPeer>();
            System.Collections.Generic.List<PlayerState> players = new System.Collections.Generic.List<PlayerState>();
            //PlayerState
            //killManager.killpl

            foreach (AirlockPeer peer in UnityEngine.Object.FindObjectsOfType<AirlockPeer>())
            {
                peers.Add(peer);
                MelonLogger.Msg($"Adding peer: {peer.PeerID}");
            }
            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                players.Add(player);
                MelonLogger.Msg($"Adding player: {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }

            PlayerRef selfPlayerRef = Helpers.GetSelfPlayerRef();

            if (peers.Count == players.Count)
            {
                foreach (PlayerState player in players)
                {
                    foreach (AirlockPeer peer in peers)
                    {
                        if (player.PlayerId == peer.PeerID)
                        {
                            Globals.killManager.KillPlayer(peer, player, selfPlayerRef, false);
                            MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
                        }
                    }
                }
            }
            else
            {
                foreach (PlayerState player in players)
                {
                    foreach (AirlockPeer peer in peers)
                    {
                        Globals.killManager.KillPlayer(peer, player, selfPlayerRef, false);
                        MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
                    }
                }
            }
        }

        public static void TestKillEveryoneMethod2()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (killEveryoneUseSelf)
            {
                selfPlayerRef.value = Helpers.GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(true, true, true, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        public static void TestKillEveryoneMethod3()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (killEveryoneUseSelf)
            {
                selfPlayerRef.value = Helpers.GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(true, false, true, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        public static void TestKillEveryoneMethod4()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (killEveryoneUseSelf)
            {
                selfPlayerRef.value = Helpers.GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(false, true, true, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        public static void TestKillEveryoneMethod5()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (killEveryoneUseSelf)
            {
                selfPlayerRef.value = Helpers.GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(true, true, false, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        public static void TestKillEveryoneMethod6()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (killEveryoneUseSelf)
            {
                selfPlayerRef.value = Helpers.GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(false, false, false, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        public static void DoKillEveryoneTestThingie()
        {
            if (killEveryoneActive)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Keypad1))
                {
                    TestKillEveryoneMethod1();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Keypad2))
                {
                    TestKillEveryoneMethod2();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) | Input.GetKeyDown(KeyCode.Keypad3))
                {
                    TestKillEveryoneMethod3();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) | Input.GetKeyDown(KeyCode.Keypad4))
                {
                    TestKillEveryoneMethod4();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) | Input.GetKeyDown(KeyCode.Keypad5))
                {
                    TestKillEveryoneMethod5();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6) | Input.GetKeyDown(KeyCode.Keypad6))
                {
                    TestKillEveryoneMethod6();
                }
            }
        }

        public static void Toggle()
        {
            killEveryoneActive = !killEveryoneActive;
            MelonLogger.Msg($"Kill everyone primed set to {killEveryoneActive}");
        }
    }
}
