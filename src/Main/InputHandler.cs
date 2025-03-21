using UnityEngine;
using AmongUsHacks.Features;

namespace AmongUsHacks.Main
{
    public static class InputHandler
    {
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
        }
    }
}
