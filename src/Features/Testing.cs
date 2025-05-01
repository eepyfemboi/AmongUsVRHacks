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

namespace AmongUsHacks.Features
{
    public class Testing
    {
        private void test1()
        {
            LobbyBrowser browser = new LobbyBrowser();
            //browser.
            //browser.StartBrowsing
            //browser.DisplayList
            ReMatchmake e1 = new ReMatchmake();
            //e1.
            PlayOnlineMenu e2 = new PlayOnlineMenu();
            //e2.
            //presen
            //e1.Remake
            AirlockPeer e3 = new AirlockPeer();
            //e3.
            //e3.QuickJoinFindGame
            //LobbySearch
            DirectJoinMenu e4 = new DirectJoinMenu();
            //e4.
            QuickMatchMenu e5 = new QuickMatchMenu();
            //e5.RegularMatchmakingFlow
            LobbySearch e6 = new LobbySearch();
            //e6.
            AirlockNetworkRunner e7 = new AirlockNetworkRunner();
            //e7.
            TestRPC e8 = new TestRPC();
            //e8.
            GlobalRPCManager e9 = new GlobalRPCManager();
            //e9.
            DebugMenu e10 = new DebugMenu();
            //e10.
            //e10.
            ModerationManager e11 = new ModerationManager();
            //e11.
            PlayerState e12 = new PlayerState();
            //e12.mod
        }
        private void ChangeNetworkName(string name)
        {
            PlayerState selfPlayerState = Helpers.GetSelfPlayerState();
            /*NetworkString<_16> nameString = new NetworkString<_16>();
            nameString.Set(name);
            selfPlayerState.NetworkName = nameString;*/
            selfPlayerState.SetNetworkName(name, true);
        }

        private void ChangePlayerModerationId(string moderationId)
        {
            PlayerState selfPlayerState = Helpers.GetSelfPlayerState();
            NetworkString<_32> moderationIdString = new NetworkString<_32>();
            moderationIdString.Set(moderationId);
            selfPlayerState.PlayerModerationID = moderationIdString;
        }
    }
}