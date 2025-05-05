using System;
using MelonLoader;
using UnityEngine;
using Il2CppFusion;
using Il2CppSG.Airlock.Network;
using Exception = System.Exception;
using AmongUsHacks.Features;
using AmongUsHacks.Main;


namespace AmongUsHacks.Data
{
    public static class Globals
    {
        public static GameObject? newInstanceDetectorObject;
        public static NetworkedKillBehaviour? killManager;
        public static NativeDebugMenu? nativeDebugMenu;
    }
}
