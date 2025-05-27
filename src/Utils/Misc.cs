using System;
using MelonLoader;
using UnityEngine;
using Il2CppFusion;
using Il2CppSG.Airlock.Network;
using Exception = System.Exception;
using AmongUsHacks.Data;
using AmongUsHacks.Features;

namespace AmongUsHacks.Utils
{
    public static class Misc
    {
        public static void InstanceChangedCheck()
        {
            if (Globals.newInstanceDetectorObject != null)
            {
                try
                {
                    string? name = Globals.newInstanceDetectorObject.name;
                    return;
                }
                catch
                {

                }
            }

            Globals.newInstanceDetectorObject = new GameObject();

            if (Speed.enabled)
            {
                Speed.enabled = false;
                Speed.Toggle();
            }
            if (Colliders.toggled)
            {
                Colliders.toggled = false;
                Colliders.Toggle();
            }
            if (ImposterReveal.enabled)
            {
                ImposterReveal.enabled = false;
                ImposterReveal.Toggle();
            }
            if (Wallhack.enabled)
            {
                Wallhack.enabled = false;
                Wallhack.Toggle();
            }
            if (KillCooldown.active)
            {
                KillCooldown.active = false;
                KillCooldown.Toggle();
            }

            AutomaticFeatures.OnInstanceChanged();
            Globals.xrInput_init = true;

            MelonLogger.Msg("Instance change detected! Resetting values.");
        }
    }
}
