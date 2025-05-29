using UnityEngine;
using AmongUsHacks.Features;
using AmongUsHacks.Data;
using Il2CppSG.Airlock.UI.TitleScreen;

namespace AmongUsHacks.Main
{
    public static class InputHandler
    {
        public static bool menu_enable_combination_pressed = false;

        public static void Handle()
        {
            if (Input.GetKeyDown(Config.CollidersToggleKey))
                Colliders.Toggle();

            if (Input.GetKeyDown(Config.SpeedToggleKey))
                Speed.Toggle();

            if (Input.GetKeyDown(Config.ImposterToggleKey))
                ImposterReveal.Toggle();

            if (Input.GetKeyDown(Config.WallHackToggleKey))
                Wallhack.Toggle();

            if (Input.GetKeyDown(Config.RainbowColorsKey))
                RainbowColors.Toggle();

            if (Input.GetKeyDown(Config.ForceImposterKey))
                ForceImposter.Execute();

            if (Input.GetKeyDown(Config.KillEveryoneActivatorKey))
                KillEveryone.Toggle();

            if (Input.GetKeyDown(Config.UnignoreGhostsKey))
                UnignoreGhosts.Toggle();

            if (Input.GetKeyDown(Config.KillCooldownKey))
                KillCooldown.Toggle();

            if (Input.GetKeyDown(Config.NativeDebugMenuKey) && Globals.nativeDebugMenu != null)
                Globals.nativeDebugMenu.ToggleNativeMenu();

            if (Input.GetKeyDown(KeyCode.M) && Globals.isVR)
                Globals.menuOp.ToggleMenu();

            if (Globals.xrInput_init && Globals.isVR) { 
                if (Globals.xrInput.IsRightGripPressed() && Globals.xrInput.IsLeftGripPressed() && !menu_enable_combination_pressed)
                {
                    menu_enable_combination_pressed = true;
                    Globals.menuOp.ToggleMenu();
                } else if (!Globals.xrInput.IsRightGripPressed() && !Globals.xrInput.IsLeftGripPressed() && menu_enable_combination_pressed)
                {
                    menu_enable_combination_pressed = false;
                }
            }
        }
    }
}
