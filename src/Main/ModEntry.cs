using MelonLoader;
using AmongUsHacks.Main;
using AmongUsHacks.Data;
using AmongUsHacks.Utils;
using System.Threading.Tasks;
using AmongUsHacks.Features;
using Il2CppSG.Airlock.UI.TitleScreen;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AmongUsHacks.Main
{
    public class ModEntry : MelonMod
    {
        public static ModEntry Instance { get; private set; }

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Howdy! Got games on your PC?");
            Instance = this;

            Config.Load();
            WebSocketClient.ConnectWebSocket();
            Task.Run(() => Updater.CheckForUpdates());
            //Task.Run(() => AssetDownloader.CheckAndUpdateBundle());
            Globals.nativeDebugMenu = new NativeDebugMenu();

            MelonLogger.Msg("COG Client Modifications loaded! || Made by MrClockwork, Fork of Sleept's AmongUsVRHacks: View the source code at https://github.com/eepyfemboi/AmongUsVRHacks");
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
