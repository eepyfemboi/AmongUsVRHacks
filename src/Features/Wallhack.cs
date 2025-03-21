using UnityEngine;
using MelonLoader;
using AmongUsHacks.Main;
using UnityEngine.Rendering.Universal;
using AmongUsHacks.Utils;
using AmongUsHacks.Data;

namespace AmongUsHacks.Features
{
    public static class Wallhack
    {
        public static bool enabled = false;
        private static System.Collections.Generic.Dictionary<GameObject, int> wallHackOriginalLayers = new System.Collections.Generic.Dictionary<GameObject, int>();
        private static UniversalAdditionalCameraData? overlayCamData;
        private static Camera? overlayCam;


        public static void Toggle()
        {
            MelonLogger.Msg($"Toggling wallhack to {!enabled}");

            if (enabled)
                Disable();
            else
                Enable();

            enabled = !enabled;

            MelonLogger.Msg($"Toggled wallhacks to {enabled}");
        }

        private static void Disable()
        {
            MelonLogger.Msg("removing wallhacks");
            RestoreOriginalLayers();
        }

        private static void Enable()
        {
            System.Collections.Generic.List<GameObject> crewmates = Helpers.GetCrewmateBodies();
            int wallHackCount = 0;
            System.Collections.Generic.List<GameObject> gameObjectsToUpdate = new System.Collections.Generic.List<GameObject>();

            SetupOverlayCamera();

            foreach (GameObject obj in crewmates)
            {
                foreach (GameObject i in Helpers.GetChildrenWithRenderers(obj))
                {
                    gameObjectsToUpdate.Add(i);
                }
            }

            ApplyOverrender(gameObjectsToUpdate);

            MelonLogger.Msg($"Enabled wallhacks for ${wallHackCount} objects");
        }

        private static void SetupOverlayCamera()
        {
            if (overlayCam != null) return;

            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                MelonLogger.Error("i think something went wrong lol");
                return;
            }

            overlayCam = new GameObject("OverlayCamera").AddComponent<Camera>();
            overlayCam.transform.SetParent(mainCam.transform);

            overlayCam.transform.localPosition = Vector3.zero;
            overlayCam.transform.localRotation = Quaternion.identity;
            overlayCam.transform.localScale = Vector3.one;

            overlayCam.depth = 100;
            overlayCam.clearFlags = CameraClearFlags.Depth;

            overlayCam.orthographic = Camera.main.orthographic;
            overlayCam.fieldOfView = Camera.main.fieldOfView;

            overlayCam.cullingMask = 1 << Config.OverlayLayer;

            overlayCam.allowHDR = false;
            overlayCam.allowMSAA = false;
            overlayCam.allowDynamicResolution = false;

            overlayCam.useOcclusionCulling = false;


            overlayCamData = overlayCam.gameObject.AddComponent<UniversalAdditionalCameraData>();
            overlayCamData.renderType = CameraRenderType.Overlay;

            UniversalAdditionalCameraData mainCamData = mainCam.GetComponent<UniversalAdditionalCameraData>();
            if (mainCamData != null)
            {
                mainCamData.cameraStack.Add(overlayCam);
                MelonLogger.Msg("it worked i think");
            }
            else
            {
                MelonLogger.Error("it didnt work");
            }


            MelonLogger.Msg("finished setting up overlay camera");
        }

        private static void ApplyOverrender(System.Collections.Generic.List<GameObject> objects)
        {
            SetupOverlayCamera();
            overlayCam.gameObject.SetActive(true);

            foreach (GameObject obj in objects)
            {
                if (obj == null) continue;

                int originalLayer = obj.layer;
                wallHackOriginalLayers[obj] = originalLayer;

                obj.layer = Config.OverlayLayer;
            }

            MelonLogger.Msg($"changed layers for {wallHackOriginalLayers.Count} objects");
        }

        private static void RestoreOriginalLayers()
        {
            SetupOverlayCamera();
            overlayCam.gameObject.SetActive(false);

            foreach (var entry in wallHackOriginalLayers)
            {
                if (entry.Key != null)
                    entry.Key.layer = entry.Value;
            }

            wallHackOriginalLayers.Clear();
            MelonLogger.Msg("restored layers");
        }
    }
}
