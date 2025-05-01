using UnityEngine;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static Il2CppFusion.NetworkCharacterController;
using UnityEngine.UI;
using MelonLoader;

public class VRPointerController : MonoBehaviour
{
    public float maxRayDistance = 10f;
    public LayerMask uiLayerMask;
    public LineRenderer lineRenderer;

    private InputDevice rightHand;
    private bool isClicking;

    void Start()
    {
        TryInitializeRightHand();

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
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
    void Update()
    {
        if (!rightHand.isValid)
            TryInitializeRightHand();

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Vector3 endpoint = origin;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxRayDistance, uiLayerMask))
        {
            endpoint = hit.point;
            TryClick(hit);
        }

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, endpoint);
        }

        //Debug.DrawLine(origin, endpoint, Color.green);
    }

    void TryInitializeRightHand()
    {
        var devices = new Il2CppSystem.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightHand = devices[0];
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

    void TryClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) && !isClicking)
        {
            isClicking = true;

            GameObject target = hit.collider.gameObject;

            var pointerEvent = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Vector2.zero,//hit.point,
                clickTime = Time.unscaledTime,
                clickCount = 1,
                button = PointerEventData.InputButton.Left,
            };

            /*ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerEnterHandler);
            ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(target, pointerEvent, ExecuteEvents.pointerUpHandler);*/
            target.GetComponent<Button>().OnPointerClick(pointerEvent);
            MelonLogger.Msg($"tried clicking {target.name}");
            //ExecuteEvents.Execute<IPointerClickHandler>(target, null, (handler, data) => handler.OnPointerClick(null));

        }
        else if (!Input.GetMouseButton(0))
        {
            isClicking = false;
        }
    }

}
