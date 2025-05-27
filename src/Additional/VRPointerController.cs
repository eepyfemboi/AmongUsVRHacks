using UnityEngine;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static Il2CppFusion.NetworkCharacterController;
using UnityEngine.UI;
using MelonLoader;
using AmongUsHacks.Utils;
using AmongUsHacks.Data;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class VRPointerController : MonoBehaviour
{
    public float maxRayDistance = 10f;
    public LayerMask uiLayerMask;
    public LineRenderer lineRenderer;

    //private InputDevice rightHand;
    private bool isClicking;

    public void Start()
    {
        TryInitializeRightHand();

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.useWorldSpace = false;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;

        MelonLogger.Msg("done with start");
    }

    /*void Update()
    {
        if (!rightHand.isValid)
            TryInitializeRightHand();

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, origin);
        }

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxRayDistance, uiLayerMask))
        {
            if (lineRenderer != null)
                lineRenderer.SetPosition(1, hit.point);

            TryClick(hit);
        }
        else
        {
            if (lineRenderer != null)
                lineRenderer.SetPosition(1, origin + direction * maxRayDistance);
        }
    }*/
    public void Update()
    {
        /*if (!rightHand.isValid)
            TryInitializeRightHand();*/

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Vector3 endpoint = origin;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxRayDistance, uiLayerMask))
        {
            endpoint = hit.point;
            //MelonLogger.Msg($"raycast hit XYZ: {endpoint.x} {endpoint.y} {endpoint.z} name: {hit.collider.gameObject.name}");
            TryClick(hit);
        }

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, endpoint);
        }

        //Debug.DrawLine(origin, endpoint, Color.green);
    }

    public void TryInitializeRightHand()
    {

        //var devices = new Il2CppSystem.Collections.Generic.List<UnityEngine.XR.InputDevice>();
        //UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);
        //var devices = UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand);

        /*Il2CppSystem.Collections.Generic.List<UnityEngine.XR.InputDevice> devices = new Il2CppSystem.Collections.Generic.List<UnityEngine.XR.InputDevice>(10);

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            rightHand = devices[0];
            MelonLogger.Msg("Right hand controller initialized.");
        }*/
        /*if (XRInputHelper.TryGetRightHand(out var device))
        {
            rightHand = device;
            MelonLogger.Msg("Right hand controller initialized.");
        }
        else
        {
            MelonLogger.Warning("No XR input device found.");
        }*/

        /*var nodeStates = new Il2CppSystem.Collections.Generic.List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (XRNodeState node in nodeStates)
        {
            if (node.nodeType == XRNode.RightHand && node.tracked)
            {
                Vector3 position;
                if (node.TryGetPosition(out position))
                {
                    //transform.position = position;
                    //MelonLogger.Msg($"Right hand position: {position}");
                    //rightHand = node.inpu;
                }
            }
        }*/

        /*if (devices.Count > 0)
        {
            rightHand = devices[0];
            MelonLogger.Msg("Right hand controller initialized.");
        }
        else
        {
            MelonLogger.Warning("No right hand XR input device found.");
        }*/

        /*var devices = new Il2CppSystem.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightHand = devices[0];*/
    }

    /*void TryClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) && !isClicking)
        {
            isClicking = true;

            var pointerEvent = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = new Vector2(Screen.width / 2, Screen.height / 2),
            };

            ExecuteEvents.Execute(hit.collider.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);
        }
        else if (!Input.GetMouseButton(0))
        {
            isClicking = false;
        }
    }*/

    public void TryClick(RaycastHit hit)
    {
        //Input.GetMouseButtonDown(0)
        if (Globals.xrInput.IsRightTriggerPressed() && !isClicking)
        {
            isClicking = true;

            GameObject target = hit.collider.gameObject;

            /*var pointerEvent = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Vector2.zero,//hit.point,
                clickTime = Time.unscaledTime,
                clickCount = 1,
                button = PointerEventData.InputButton.Left,
            };*/

            /*ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerEnterHandler);
            ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerUpHandler);*/
            target.GetComponent<Button>().Press();
            MelonLogger.Msg($"tried clicking {target.name}");
            //ExecuteEvents.Execute<IPointerClickHandler>(target, null, (handler, data) => handler.OnPointerClick(null));

        }
        else if (!Globals.xrInput.IsRightTriggerPressed())
        {
            isClicking = false;
        }
    }

}
