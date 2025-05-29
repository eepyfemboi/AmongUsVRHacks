using System;
using MelonLoader;
using UnityEngine;
using Il2CppFusion;
using Il2CppSG.Airlock.Network;
using Exception = System.Exception;
using AmongUsHacks.Features;
using AmongUsHacks.Main;
using AmongUsHacks.Utils;


namespace AmongUsHacks.Data
{
    public static class Globals
    {
        public static GameObject? newInstanceDetectorObject;
        public static NetworkedKillBehaviour? killManager;
        public static NativeDebugMenu? nativeDebugMenu;
        public static UIMenuOperator? menuOp;
        public static XRInputWrapper? xrInput;
        public static bool xrInput_init = false;
        public static bool isVR = false;
    }
}
