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
using Il2CppSG.Airlock.UI;

namespace AmongUsHacks.Features
{
    public static class UnlockHiddenFeatures
    {
        public static void EnableHiddenOptions()
        {
            try
            {
                foreach (UIRoleDescription roleDes in GameObject.FindObjectsOfType<UIRoleDescription>())
                {
                    roleDes.gameObject.SetActive(true);
                }
                foreach (UIMatchOption roleOpt in GameObject.FindObjectsOfType<UIMatchOption>())
                {
                    roleOpt.gameObject.SetActive(true);
                }
            } catch
            {
                //
            }
        }
    }
}