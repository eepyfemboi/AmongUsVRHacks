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


namespace AmongUsHacks.Additional
{
    public class XRInputHelper
    {
        /*public static bool TryGetRightHand(out InputDevice device)
        {
            /*var devices = new Il2CppSystem.Collections.Generic.List<UnityEngine.XR.InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
            if (devices.Count > 0)
            {
                device = devices[0];
                return true;
            }
            device = default;
            return false;
            List<XRNodeState> nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);

            foreach (XRNodeState node in nodeStates)
            {
                if (node.nodeType == XRNode.RightHand && node.tracked)
                {
                    Vector3 position;
                    if (node.TryGetPosition(out position))
                    {
                        transform.position = position;
                        MelonLogger.Msg($"Right hand position: {position}");
                    }
                }
            }
        }*/

        /*public bool IsRightTriggerPressed1()
        {
            var rightHand = InputSystem.GetDevice<XRController>(UnityEngine.InputSystem.CommonUsages.RightHand);

            if (rightHand == null)
            {
                MelonLogger.Warning("Right hand controller not found.");
                return false;
            }

            //XRNode.
            //InputDeviceRole.RightHanded
            //XRNode.RightHand

            if (rightHand.TryGetFeatureValue(UnityEngine.InputSystem.CommonUsages.triggerButton, out bool triggerPressed))
            {
                return triggerPressed;
            }

            return false;
        }*/

        /*public bool IsRightTriggerPressed()
        {
            var device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool triggerValue;
            if (device.TryGetFeatureUsages(UnityEngine.InputSystem.CommonUsages.trigger, out triggerValue) && triggerValue)
            {
                Debug.Log("Trigger button is pressed");
            }
            return triggerValue;
        }*/

        /*public static bool IsRightTriggerPressed()
        {
            foreach (var device in InputSystem.devices)
            {
                if (device is XRController controller &&
                    controller.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                {
                    var triggerButton = controller["trigger"];
                    if (triggerButton is ButtonControl buttonControl)
                    {
                        return buttonControl.isPressed;
                    }
                }
            }

            return false;
        }*/
        public static bool IsRightTriggerPressed()
        {
            string output_log_string = "";

            bool is_pressed = false;

            foreach (var device in InputSystem.devices)
            {
                string current_device_log_str = $"Name: {device.name} || {device.displayName}";
                var new_devices = new List<UnityEngine.InputSystem.Utilities.InternedString>();
                bool is_right_hand = false;
                foreach (var new_device in device.usages)
                {
                    new_devices.Add(new_device);
                    //MelonLogger.Msg($"Device:")
                    //current_device_log_str += $"; {new_device.ToString()}";
                    if (new_device.ToString().ToLower() == "righthand")
                    {
                        is_right_hand = true;
                    }
                }


                if (is_right_hand && device.name == "SteamXRController")
                {
                    foreach(var cont in device.allControls)
                    {
                        current_device_log_str += $"\n{cont.displayName} || {cont.name} || {cont.isButton} || {cont.IsPressed()} || {cont.path}";
                    }

                    var trigger = device.TryGetChildControl("trigger");

                    MelonLogger.Msg(current_device_log_str);

                    if (trigger is ButtonControl button)
                        is_pressed = button.IsPressed();
                }
                //MelonLogger.Msg(current_device_log_str);
            }

            //output_log_string += ";; couldnt find trigger";

            //MelonLogger.Msg(output_log_string);

            return is_pressed;
        }

    }
}
