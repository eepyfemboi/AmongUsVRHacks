﻿using MelonLoader;
using AmongUsHacks.Main;
using AmongUsHacks.Data;
using AmongUsHacks.Utils;
using System.Threading.Tasks;
using AmongUsHacks.Features;

namespace AmongUsHacks.Main
{
    public class ModEntry : MelonMod
    {
        public static ModEntry Instance { get; private set; }

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("hi");
            Instance = this;
            Config.Load();
            BlacklistManager.Load();
            WebSocketClient.ConnectWebSocket();
            Task.Run(() => Updater.CheckForUpdates());
            //Task.Run(() => AssetDownloader.CheckAndUpdateBundle());
            Globals.nativeDebugMenu = new NativeDebugMenu();
            Globals.menuOp = new UIMenuOperator();

            MelonLogger.Msg("Sleepy's AmongUsVR Hacks Loaded! View the source code at https://github.com/eepyfemboi/AmongUsVRHacks");
        }

        public override void OnUpdate()
        {
            InputHandler.Handle();
            Misc.InstanceChangedCheck();
            RainbowColors.DoRainbowColors();
            KillEveryone.DoKillEveryoneTestThingie();
            UnignoreGhosts.DoUpdate();
            KillCooldown.UpdateCooldown();
            if (Globals.nativeDebugMenu != null)
            {
                Globals.nativeDebugMenu.DoUpdateCheck();
            }
        }
    }
}
