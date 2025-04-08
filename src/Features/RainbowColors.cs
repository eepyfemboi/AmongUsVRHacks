using System;
using MelonLoader;
using UnityEngine;
using Il2CppFusion;
using Il2CppSG.Airlock.Network;
using Exception = System.Exception;
using Il2CppSG.Airlock;


namespace AmongUsHacks.Features
{
    public static class RainbowColors
    {
        private static int colorsAmount = 12;
        private static int currentColor = 0;

        public static bool enabled = false;

        private static System.Collections.Generic.List<PlayerState>? rainbowPlayers = null;
        private static int rainbowFrame = 0;
        private static int maxRainbowFrames = 20;

        public static void DoRainbowColors()
        {
            if (enabled)
            {
                if (rainbowPlayers != null)
                {
                    if (rainbowFrame == maxRainbowFrames)
                    {
                        if (currentColor == colorsAmount)
                        {
                            currentColor = 0;
                        }

                        foreach (PlayerState player in rainbowPlayers)
                        {
                            //player.UpdateColorID(currentColor);
                            player.ColorId = currentColor;
                            //player.RPC_ForceCheckColor //harmony patch this later ig idk
                        }

                        currentColor++;
                        rainbowFrame = 0;
                    }
                    else
                    {
                        rainbowFrame++;
                    }
                }
            }
        }

        public static void Toggle()
        {
            rainbowPlayers = new System.Collections.Generic.List<PlayerState>();
            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                rainbowPlayers.Add(player);
            }

            enabled = !enabled;
            MelonLogger.Msg($"Toggled rainbow colors to {enabled}");
        }
    }
}
