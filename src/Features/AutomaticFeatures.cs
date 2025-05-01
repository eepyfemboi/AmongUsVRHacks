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
using Il2CppFusion.CodeGen;
using Il2CppSG.Airlock.UI.Moderation;

namespace AmongUsHacks.Features
{
    public static class AutomaticFeatures
    {
        public static void OnInstanceChanged()
        {
            SpoofModerationId();
            UnlockHiddenFeatures.EnableHiddenOptions();
        }

        private static void SpoofModerationId()
        {
            try
            {
                PlayerState selfPlayerState = Helpers.GetSelfPlayerState();
                string newModerationId = "https://eepy.io/";
                //NetworkString<_32> moderationIdString = new NetworkString<_32>();
                //moderationIdString.Set("https://eepy.io/");
                //selfPlayerState.PlayerModerationID = moderationIdString;
                selfPlayerState.PlayerModerationID.Set(newModerationId);
            } catch
            {
                //
            }
        }
    }
}