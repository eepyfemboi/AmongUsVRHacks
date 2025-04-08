using UnityEngine;
using MelonLoader;
using Il2CppSG.Airlock.Network;
using Il2Cpp;
using Il2CppFusion;
using Il2CppSG.Airlock;
using AmongUsHacks.Utils;
using AmongUsHacks.Data;
using Il2CppSG.Airlock.UI.TitleScreen;
using Il2CppEpic.OnlineServices.Lobby;
using Il2CppSG.Airlock.Util;
using Il2CppSG.Airlock.XR;

namespace AmongUsHacks.Features
{
    public class NativeDebugMenu
    {
        public bool nativeDebugMenuEnabled = false;
        private GameObject? debugMenuObj = null;
        private DebugMenu? debugMenu = null;
        private GameObject? debugMenuPrefab = null;

        private bool EnsureDebugMenuPrefab()
        {
            if (debugMenuPrefab != null) return true;

            GameObject? prefabTemp = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "DebugMenu");
            if (prefabTemp == null)
            {
                MelonLogger.Error("Could not find native debug menu prefab.");
                return false;
            }
            MelonLogger.Msg("found debug menu prefab");

            debugMenuPrefab = prefabTemp;
            return true;
        }

        private void EnsureSetToHand()
        {
            GameObject? hand = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(go => go.name == "LeftHand");
            if (hand == null)
            {
                MelonLogger.Error("Could not find left hand");
                return;
            }

            DebugMenu? tempMenuTst = hand.GetComponentInChildren<DebugMenu>();
            if (tempMenuTst != null) return;

            debugMenuObj.transform.SetParent(hand.transform, false);
            Vector3 localPos = new Vector3(-0.2f, 0f, 0.2f);
            //Quaternion localRot = new Quaternion(50, 310, 0)
            Vector3 localRot = new Vector3(50f, 310f, 0f);
            Vector3 localSca = new Vector3(0.0015f, 0.0015f, 0.0015f);
            //debugMenuObj.transform.set_localPosition_Injected(localPos);
            //debugMenuObj.transform.set_localRotation_Injected()
            debugMenuObj.transform.localPosition = localPos;
            debugMenuObj.transform.localEulerAngles = localRot;
            debugMenuObj.transform.localScale = localSca;
        }

        private void TrySetInteractable()
        {
            //
        }

        private bool EnsureNativeDebugMenu()
        {
            if (debugMenuObj != null && debugMenu != null)
            {
                debugMenuObj.SetActive(true);
                return true;
            }

            if (!EnsureDebugMenuPrefab()) return false;

            if (debugMenuObj != null)
            {
                GameObject.Destroy(debugMenuObj);
            }

            GameObject clone = GameObject.Instantiate(debugMenuPrefab);
            clone.name = "DebugMenu_Clone";
            clone.SetActive(true);
            
            debugMenuObj = clone;
            debugMenu = debugMenuObj.GetComponent<DebugMenu>();

            EnsureSetToHand();

            return true;
        }

        public void DoUpdateCheck()
        {
            if (nativeDebugMenuEnabled)
            {
                EnsureNativeDebugMenu();
            }
        }

        public void ToggleNativeMenu()
        {
            if (nativeDebugMenuEnabled && debugMenuObj != null)
            {
                debugMenuObj.SetActive(false);
            } else
            {
                EnsureNativeDebugMenu();
            }
            nativeDebugMenuEnabled = !nativeDebugMenuEnabled;
            MelonLogger.Msg($"Set native debug menu value to {nativeDebugMenuEnabled}");
        }
    }
}