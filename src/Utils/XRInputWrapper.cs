using UnityEngine;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static Il2CppFusion.NetworkCharacterController;
using UnityEngine.UI;
using MelonLoader;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using AmongUsHacks.Data;


namespace AmongUsHacks.Utils
{
    public class XRInputWrapper
    {
        public UnityEngine.InputSystem.InputDevice RightHand;
        public UnityEngine.InputSystem.InputDevice LeftHand;
        private string rhand_path_prefix = "";
        private string lhand_path_prefix = "";

        private string control_path_prefix = "/SteamXRController/";

        public void FindInputDevicesHands()
        {
            foreach (var device in InputSystem.devices)
            {
                if (device.name == "SteamXRController" | device.name == "SteamXRController1")
                {
                    foreach (var usage in device.usages)
                    {
                        if (usage.ToString().ToLower() == "righthand")
                        {
                            RightHand = device;
                            rhand_path_prefix = $"/{device.name}/";
                            MelonLogger.Msg("found right hand");
                        } else if (usage.ToString().ToLower() == "lefthand")
                        {
                            LeftHand = device;
                            lhand_path_prefix = $"/{device.name}/";
                            MelonLogger.Msg("found left hand");
                        }
                    }
                }
            }
        }

        public void DoTestThingie()
        {
            foreach (var device in InputSystem.devices)
            {
                string current_device_log_str = $"Name: {device.name} || {device.displayName}";

                foreach (var cont in device.allControls)
                {
                    current_device_log_str += $"\n{cont.displayName} || {cont.name} || {cont.isButton} || {cont.IsPressed()} || {cont.path}";
                }

                MelonLogger.Msg(current_device_log_str);
            }
        }

        private void InitializeValues()
        {
            if (RightHand == null || LeftHand == null)
            {
                FindInputDevicesHands();
            }
            Globals.xrInput_init = true;
            //DoTestThingie();
        }

        public bool IsPathPressed(int handVal, string path)
        {
            InitializeValues();

            UnityEngine.InputSystem.InputDevice hand = LeftHand;
            string prefix = "";

            if (handVal == 0)
            {
                hand = LeftHand;
                prefix = lhand_path_prefix;
            } else if (handVal == 1)
            {
                hand = RightHand;
                prefix = rhand_path_prefix;
            }

            /*var control = hand.TryGetChildControl(control_path_prefix + path);
            if (control != null)
            {
                MelonLogger.Msg(control.path);
                return control.IsPressed();
            }*/
            foreach (var control in hand.allControls)
            {
                if (control.path == prefix + path)
                {
                    return control.IsPressed();
                }
            }
            return false;
        }


        // === Left Hand Methods ===
        public bool IsLeftPrimaryButtonPressed() => IsPathPressed(0, "primaryButton");
        public bool IsLeftSecondaryButtonPressed() => IsPathPressed(0, "secondaryButton");
        public bool IsLeftThumbstickClicked() => IsPathPressed(0, "thumbstickClicked");
        public bool IsLeftThumbstickTouched() => IsPathPressed(0, "thumbstickTouched");
        public bool IsLeftTriggerTouched() => IsPathPressed(0, "triggerTouched");
        public bool IsLeftTriggerPressed() => IsPathPressed(0, "triggerPressed");
        public bool IsLeftGripPressed() => IsPathPressed(0, "gripPressed");
        public bool IsLeftThumbstickUp() => IsPathPressed(0, "thumbstick/up");
        public bool IsLeftThumbstickDown() => IsPathPressed(0, "thumbstick/down");
        public bool IsLeftThumbstickLeft() => IsPathPressed(0, "thumbstick/left");
        public bool IsLeftThumbstickRight() => IsPathPressed(0, "thumbstick/right");

        // === Right Hand Methods ===
        public bool IsRightPrimaryButtonPressed() => IsPathPressed(1, "primaryButton");
        public bool IsRightSecondaryButtonPressed() => IsPathPressed(1, "secondaryButton");
        public bool IsRightThumbstickClicked() => IsPathPressed(1, "thumbstickClicked");
        public bool IsRightThumbstickTouched() => IsPathPressed(1, "thumbstickTouched");
        public bool IsRightTriggerTouched() => IsPathPressed(1, "triggerTouched");
        public bool IsRightTriggerPressed() => IsPathPressed(1, "triggerPressed");
        public bool IsRightGripPressed() => IsPathPressed(1, "gripPressed");
        public bool IsRightThumbstickUp() => IsPathPressed(1, "thumbstick/up");
        public bool IsRightThumbstickDown() => IsPathPressed(1, "thumbstick/down");
        public bool IsRightThumbstickLeft() => IsPathPressed(1, "thumbstick/left");
        public bool IsRightThumbstickRight() => IsPathPressed(1, "thumbstick/right");
    }
}
