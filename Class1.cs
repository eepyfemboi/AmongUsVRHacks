﻿using MelonLoader;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Collections;
using Il2CppSG.Airlock.XR;
using Il2CppSG.Airlock;
using static Il2Cpp.Utils;
using Il2CppSG.Airlock.Roles;
using Il2CppFusion;
using Il2CppSG.Airlock.UI;
using Il2CppSG.Airlock.Network;
using Il2CppSystem.Collections.Generic;
using MelonLoader.Utils;
using System.Net.Http;
using Il2CppSystem;
using Exception = System.Exception;
using Il2CppSG.Airlock.Util;
using UnityEngine.Rendering;
using Il2Cpp;
using UnityEngine.Rendering.Universal;

namespace AmongUsHacks
{
    public class Class1 : MelonMod
    {
        private MelonPreferences_Category? settings;


        private bool blacklistUpdated = false;
        private string remoteBlacklistURL = "https://raw.githubusercontent.com/eepyfemboi/AmongUsVRHacks/refs/heads/main/dynamic_data/blacklisted_object_names.txt";
        private bool doRemoteBlacklistUpdate = true;

        private MelonPreferences_Entry<string>? remoteBlacklistURL_config;
        private MelonPreferences_Entry<bool>? doRemoteBlacklistUpdate_config;


        private string blacklistFilePath = Path.Combine(MelonEnvironment.GameRootDirectory, "blacklist.txt");
        private System.Collections.Generic.List<string> blacklist = new System.Collections.Generic.List<string>();
        private KeyCode collidersToggleKey = KeyCode.C;
        private bool collidersToggled = false;
        private System.Collections.Generic.List<GameObject> disabledObjectsList = new System.Collections.Generic.List<GameObject>();

        private MelonPreferences_Entry<KeyCode>? collidersToggleKey_config;


        private KeyCode speedToggleKey = KeyCode.S;
        private bool speedEnabled = false;
        private float speedSetting = 11f;

        private MelonPreferences_Entry<KeyCode>? speedToggleKey_config;
        private MelonPreferences_Entry<float>? speedSetting_config;


        private KeyCode wallHackToggleKey = KeyCode.W;
        private bool wallHackEnabled = false;
        private System.Collections.Generic.Dictionary<Renderer, Material[]> modifiedRenderers = new System.Collections.Generic.Dictionary<Renderer, Material[]>();
        private System.Collections.Generic.Dictionary<Renderer, Material[]> tempMaterials = new System.Collections.Generic.Dictionary<Renderer, Material[]>();
        private System.Collections.Generic.Dictionary<Shader, System.Collections.Generic.Dictionary<string, float>> savedShaderProperties = new System.Collections.Generic.Dictionary<Shader, System.Collections.Generic.Dictionary<string, float>>();
        private System.Collections.Generic.Dictionary<Shader, int> savedRenderQueues = new System.Collections.Generic.Dictionary<Shader, int>();
        private string wallHackShaderModifyName = "Airlock/Character_V2";
        private Camera overlayCam;
        private System.Collections.Generic.List<GameObject> wallHackTargetObjects = new System.Collections.Generic.List<GameObject>();
        private System.Collections.Generic.Dictionary<GameObject, int> wallHackOriginalLayers = new System.Collections.Generic.Dictionary<GameObject, int>();
        private UniversalAdditionalCameraData overlayCamData;
        private int overlayLayer = 7;

        private MelonPreferences_Entry<KeyCode>? wallHackToggleKey_config;
        private MelonPreferences_Entry<int>? overlayLayer_config;


        private KeyCode imposterToggleKey = KeyCode.I;
        private bool imposterEnabled = false;

        private MelonPreferences_Entry<KeyCode>? imposterToggleKey_config;


        private GameObject? newInstanceDetectorObject;


        public override void OnInitializeMelon()
        {
            LoadConfig();

            MelonLogger.Msg("Sleepy's AmongUsVR Hacks Loaded! View the source at https://github.com/eepyfemboi/AmongUsVRHacks");
            LoadBlacklist();
        }

        private void LoadConfig()
        {
            settings = MelonPreferences.CreateCategory("AmongUsVRHacksSettings");


            remoteBlacklistURL_config = settings.CreateEntry("RemoteBlacklistURL", remoteBlacklistURL, "Remote Object Blacklist URL");
            doRemoteBlacklistUpdate_config = settings.CreateEntry("DoRemoteBlacklistUpdate", doRemoteBlacklistUpdate, "Do Remote Blacklist Update On Load");

            collidersToggleKey_config = settings.CreateEntry("ToggleCollidersKeybind", collidersToggleKey, "Toggle Colliders Keybind");

            speedToggleKey_config = settings.CreateEntry("ToggleSpeedKeybind", speedToggleKey, "Toggle Speed Keybind");
            speedSetting_config = settings.CreateEntry("SpeedSetting", speedSetting, "Speed Increase Value");

            wallHackToggleKey_config = settings.CreateEntry("WallHackToggleKey", wallHackToggleKey, "Toggle Wallhacks Keybind");
            overlayLayer_config = settings.CreateEntry("WallHackOverlayLayer", overlayLayer, "WallHack Overlay Layer");

            imposterToggleKey_config = settings.CreateEntry("ImposterToggleKey", imposterToggleKey, "Toggle Show Imposters Keybind");


            remoteBlacklistURL = remoteBlacklistURL_config.Value;
            doRemoteBlacklistUpdate = doRemoteBlacklistUpdate_config.Value;

            collidersToggleKey = collidersToggleKey_config.Value;

            speedToggleKey = speedToggleKey_config.Value;
            speedSetting = speedSetting_config.Value;

            wallHackToggleKey = wallHackToggleKey_config.Value;
            overlayLayer = overlayLayer_config.Value;

            imposterToggleKey = imposterToggleKey_config.Value;
        }

        public override void OnUpdate()
        {
            HandleKeybinds();
            InstanceChangedCheck();
        }

        private void HandleKeybinds()
        {
            if (Input.GetKeyDown(collidersToggleKey))
            {
                ToggleColliders();
            }
            if (Input.GetKeyDown(speedToggleKey))
            {
                TogglePlayerSpeed();
            }
            if (Input.GetKeyDown(imposterToggleKey))
            {
                ToggleShowImposters();
            }
            if (Input.GetKeyDown(wallHackToggleKey))
            {
                ToggleWallHack();
            }
        }



        private void ToggleColliders()
        {
            MelonLogger.Msg($"Toggling colliders to {!collidersToggled}");

            LoadBlacklist();
            if (collidersToggled)
            {
                ReEnableDisabledObjects();
            }
            else
            {
                DisableMatchingObjects();
            }

            collidersToggled = !collidersToggled;
            MelonLogger.Msg($"Toggling colliders to {collidersToggled}");
        }

        private void TogglePlayerSpeed()
        {
            MelonLogger.Msg($"Toggling speed increase to {!speedEnabled}");

            if (speedEnabled)
            {
                SetPlayerSpeed(6.5f);
            }
            else
            {
                SetPlayerSpeed(speedSetting);
            }

            speedEnabled = !speedEnabled;
            MelonLogger.Msg($"Toggled speed increase to {speedEnabled}");
        }

        private void ToggleShowImposters()
        {
            MelonLogger.Msg($"Toggling show imposters to {!imposterEnabled}");

            if (imposterEnabled)
            {
                HideImposters();
            }
            else
            {
                ShowImposters();
            }

            imposterEnabled = !imposterEnabled;
            MelonLogger.Msg($"Toggled show imposters to {imposterEnabled}");
        }

        private void ToggleWallHack()
        {
            MelonLogger.Msg($"Toggling wallhack to {!wallHackEnabled}");

            if (wallHackEnabled)
            {
                DisableWallHack();
            }
            else
            {
                EnableWallHack();
            }

            wallHackEnabled = !wallHackEnabled;
            MelonLogger.Msg($"Toggled wallhack to {wallHackEnabled}");
        }



        private void FetchBlacklist()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    MelonLogger.Msg($"Updating blacklisted objects from url: {remoteBlacklistURL}");
                    string response = client.GetStringAsync(remoteBlacklistURL).GetAwaiter().GetResult();
                    File.WriteAllText(blacklistFilePath, response);
                    MelonLogger.Msg("Finished updating blacklisted objects");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"Failed to fetch blacklisted objects: {ex.Message}");
            }

            blacklistUpdated = true;
        }

        private void LoadBlacklist()
        {
            if (!blacklistUpdated && doRemoteBlacklistUpdate)
            {
                MelonLogger.Msg("Attempting to fetch updated blacklist...");
                FetchBlacklist();
            }

            if (File.Exists(blacklistFilePath))
            {
                blacklist = File.ReadAllLines(blacklistFilePath)
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line))
                    .ToList();

                MelonLogger.Msg($"Loaded {blacklist.Count} blacklist entries.");
            }
            else
            {
                MelonLogger.Warning($"Blacklist file not found: {blacklistFilePath}");
            }
        }



        private void SetImposterNametag(NetworkedLocomotionPlayer player, bool isImposter)
        {
            UINameTag nameTag = player._nameTag;
            nameTag.IsImpostorText = isImposter;
        }

        private void HideImposters()
        {
            foreach (NetworkedLocomotionPlayer player in GameObject.FindObjectsOfType<NetworkedLocomotionPlayer>())
            {
                try
                {
                    SetImposterNametag(player, false);
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg(ex);
                }
            }
        }

        private void ShowImposters()
        {
            RoleManager roleManager = GameObject.FindObjectOfType<RoleManager>();
            //List<int> imposterPlayerIDs = roleManager.gameRoleToPlayerIds.TryGetValue(GameRole.Imposter, out List<int> imposterPlayerIDs);
            //roleManager.gameRoleToPlayerIds;
            
            //roleManager.CheckPlayerRole(GameRole.Imposter)
            if (roleManager.gameRoleToPlayerIds.TryGetValue(GameRole.Imposter, out Il2CppSystem.Collections.Generic.List<int> imposterPlayerIDs))
            {
                MelonLogger.Msg($"Raw Dict: {roleManager.gameRoleToPlayerIds.ToString()}  Gathered Player IDs: {imposterPlayerIDs}");
                foreach (NetworkedLocomotionPlayer player in GameObject.FindObjectsOfType<NetworkedLocomotionPlayer>())
                {
                    try
                    {
                        //player._cachedPlayerID
                        //player._nameTag.NameID
                        MelonLogger.Msg($"Current Player ID: {player._cachedPlayerID}");
                        if (imposterPlayerIDs.Contains(player._cachedPlayerID))
                        {
                            MelonLogger.Msg($"Setting player ID {player._cachedPlayerID} name {player._nameTag._storedName} as imposter");
                            SetImposterNametag(player, true);
                            MelonLogger.Msg($"Player ID {player._cachedPlayerID} name {player._nameTag._storedName} has been set as imposter");
                        }
                    } catch (Exception ex)
                    {
                        MelonLogger.Msg(ex);
                    }
                }
            }
        }



        private void DisableMatchingObjects()
        {
            int disabledCount = 0;
            disabledObjectsList = new System.Collections.Generic.List<GameObject>();

            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (blacklist.Any(phrase => obj.name.IndexOf(phrase, System.StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    obj.SetActive(false);
                    disabledObjectsList.Add(obj);
                    disabledCount++;
                    MelonLogger.Msg($"Disabled: {obj.name}");
                }
            }

            MelonLogger.Msg($"Finished disabling {disabledCount} objects.");
        }

        private void ReEnableDisabledObjects()
        {
            int enabledCount = 0;
            try
            {
                foreach (GameObject obj in disabledObjectsList)
                {
                    try
                    {
                        obj.SetActive(true);
                        enabledCount++;
                        MelonLogger.Msg($"Re-Enabled: {obj.name}");
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Msg($"Failed to re-enable object: {obj.name}");
                    }
                }
            }
            catch (Exception ex) 
            {
                MelonLogger.Msg(ex);
            }

            MelonLogger.Msg($"Finished re-enabling {enabledCount} objects");
        }



        private void SetPlayerSpeed(float speed)
        {
            int modifiedCount = 0;
            foreach (XRRig rig in GameObject.FindObjectsOfType<XRRig>())
            {
                rig._speed = speed;
                modifiedCount++;
                MelonLogger.Msg($"Modified {rig.gameObject.name}: _speed set to {speed}");
            }

            MelonLogger.Msg($"Finished updating {modifiedCount} XRRig instances.");
        }


        private System.Collections.Generic.List<GameObject> FindAllCrewmatePhysics()
        {
            System.Collections.Generic.List<GameObject> crewmates = new System.Collections.Generic.List<GameObject>();

            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (obj.name.Contains("CrewmatePhysics"))
                {
                    crewmates.Add(obj);
                    MelonLogger.Msg($"Found: {obj.name}");
                }
            }

            return crewmates;
        }

        private GameObject? FindSelfCrewmatePhysics(System.Collections.Generic.List<GameObject> crewmates)
        {
            foreach (GameObject obj in crewmates)
            {
                foreach (Transform childObj in obj.GetComponentsInChildren<Transform>()) 
                { 
                    if (childObj.gameObject.name == "Speaker")
                    {
                        return childObj.gameObject;
                    }
                }
            }

            return null;
        }

        private void DisableFocusLostNotif()
        {
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (obj.name == "SM_UI_NotificationPopup_01")
                {
                    UnityEngine.Object.Destroy(obj);
                }
                if (obj.name == "UI_FocusLostNotification")
                {
                    MaterialProperty objMat = obj.GetComponent<MaterialProperty>();
                    objMat.enabled = false;
                }
            }
        }

        private void ApplyWallHackToObject(GameObject obj)
        {
            int currentChangedRenderers = 0;

            foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
            {
                Material[] originalMaterials = renderer.sharedMaterials;
                //Material[] originalMaterials = renderer.materials;
                Material[] duplicatedMaterials = new Material[originalMaterials.Length];

                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    if (originalMaterials[i] == null)
                    {
                        MelonLogger.Warning($"the material is missing for renderer {renderer.gameObject.name}!");
                        continue;
                    }

                    duplicatedMaterials[i] = new Material(originalMaterials[i]);
                    /*{
                        shader = originalMaterials[i].shader,
                        renderQueue = 5000
                    };*/

                    Shader originalShader = originalMaterials[i].shader;
                    Shader newShader = new Shader();
                    newShader = Shader.Find(originalShader.name);

                    duplicatedMaterials[i].shader = newShader;

                    // at this point im just trying everything i can find online to make this work because i dont want to have to make a custom shader for the wallhacks
                    duplicatedMaterials[i].renderQueue = 5000;
                    duplicatedMaterials[i].SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
                    duplicatedMaterials[i].SetInt("_ZWrite", 0);
                    duplicatedMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    duplicatedMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    duplicatedMaterials[i].SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }

                renderer.materials = duplicatedMaterials;
                modifiedRenderers[renderer] = originalMaterials;
                tempMaterials[renderer] = duplicatedMaterials;
            }

            MelonLogger.Msg($"enabled wallhack for {modifiedRenderers.Count} renderers");
        }

        private void RemoveWallHacks()
        {
            foreach (var entry in modifiedRenderers)
            {
                Renderer renderer = entry.Key;
                Material[] originalMaterials = entry.Value;

                if (renderer != null)
                {
                    renderer.materials = originalMaterials;
                }
            }

            /*foreach (Renderer renderer in modifiedRenderers.Keys)
            {
                foreach (Material mat in renderer.materials)
                {
                    UnityEngine.Object.Destroy(mat);
                }
            }

            modifiedRenderers.Clear();*/

            foreach (var entry in tempMaterials)
            {
                foreach (Material mat in entry.Value)
                {
                    if (mat != null)
                        UnityEngine.Object.Destroy(mat);
                }
            }

            modifiedRenderers.Clear();
            tempMaterials.Clear();

            MelonLogger.Msg("restored original materials from wallhack");
        }


        private void SetupOverlayCamera()
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

            overlayCam.cullingMask = 1 << overlayLayer;

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

            //UniversalAdditionalCameraData baseCameraData = mainCam.GetComponent<UniversalAdditionalCameraData>();
            //baseCameraData.cameraStack.Add(overlayCam);

            MelonLogger.Msg("finished setting up overlay camera");
        }

        private void ApplyOverrender(System.Collections.Generic.List<GameObject> objects)
        {
            SetupOverlayCamera();
            overlayCam.gameObject.SetActive(true);

            foreach (GameObject obj in objects)
            {
                if (obj == null) continue;

                int originalLayer = obj.layer;
                wallHackOriginalLayers[obj] = originalLayer;

                obj.layer = overlayLayer;
            }

            MelonLogger.Msg($"changed layers for {wallHackOriginalLayers.Count} objects");
        }

        private void RestoreOriginalLayers()
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

        private bool HasSpeakerGameObject1(GameObject obj)
        {
            foreach (Transform i in obj.transform)
            {
                if (i.gameObject.name == "Speaker") return true;
                if (HasSpeakerGameObject1(i.gameObject)) return true;
            }
            return false;
        }

        private bool HasSpeakerGameObject(GameObject obj)
        {
            foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
            {
                if (child.gameObject.name == "Speaker") return true;
            }
            return false;
        }

        private int GetSelfPlayerID()
        {
            int playerId = -1;

            System.Collections.Generic.List<NetworkedLocomotionPlayer> realPlayers = new System.Collections.Generic.List<NetworkedLocomotionPlayer>();
            System.Collections.Generic.List<NetworkedLocomotionPlayer> currentPlayers = new System.Collections.Generic.List<NetworkedLocomotionPlayer>();

            foreach (NetworkedLocomotionPlayer player in GameObject.FindObjectsOfType<NetworkedLocomotionPlayer>()) //im fuckin tired asf rn and i know this is terrible but i really couldnt care less so ill probably (not) improve it later
            {
                currentPlayers.Add(player);
            }

            foreach(NetworkedLocomotionPlayer player in currentPlayers)
            {
                try
                {
                    if (player._cachedPlayerID < 0)
                    {
                        continue;
                    }
                    realPlayers.Add(player);
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg(ex);
                }
            }

            foreach(NetworkedLocomotionPlayer player in realPlayers)
            {
                if (HasSpeakerGameObject(player.gameObject))
                {
                    playerId = player._cachedPlayerID;
                    MelonLogger.Msg($"assuming self id is {playerId}");
                }
            }

            return playerId;
        }

        private bool CheckIfCrewmateIsSelf(GameObject obj, int selfPlayerId)
        {
            NetworkedLocomotionPlayer current = obj.GetComponentInParent<NetworkedLocomotionPlayer>();
            if (current._cachedPlayerID == selfPlayerId)
            {
                return true;
            }
            return false;
        }


        private System.Collections.Generic.List<GameObject> GetChildrenWithRenderers(GameObject parent)
        {
            System.Collections.Generic.List<GameObject> objectsWithRenderers = new System.Collections.Generic.List<GameObject>();

            foreach (Renderer renderer in parent.GetComponentsInChildren<Renderer>(true))
            {
                objectsWithRenderers.Add(renderer.gameObject);
            }

            return objectsWithRenderers;
        }


        private System.Collections.Generic.List<GameObject> GetCrewmateBodies()
        {
            System.Collections.Generic.List<GameObject> crewmateObjectsList = new System.Collections.Generic.List<GameObject>();

            int selfPlayerId = GetSelfPlayerID();
            MelonLogger.Msg($"ASsuming self player id is {selfPlayerId}");

            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (obj.name.Contains("CrewmatePhysics"))
                {
                    if (!CheckIfCrewmateIsSelf(obj, selfPlayerId))
                    {
                        crewmateObjectsList.Add(obj);
                    }
                }
            }
            return crewmateObjectsList;
        }

        private void DisableWallHack()
        {
            MelonLogger.Msg("removing wallhacks");
            //RemoveWallHacks();
            RestoreOriginalLayers();
        }

        private void EnableWallHack()
        {
            System.Collections.Generic.List<GameObject> crewmates = GetCrewmateBodies();
            int wallHackCount = 0;
            System.Collections.Generic.List<GameObject> gameObjectsToUpdate = new System.Collections.Generic.List<GameObject>();

            /*foreach(GameObject obj in crewmates)
            {
                MelonLogger.Msg($"applying wallhack to ${obj.name}");
                ApplyWallHackToObject(obj);
                wallHackCount++;
            }*/
            SetupOverlayCamera();

            foreach (GameObject obj in crewmates)
            {
                foreach (GameObject i in GetChildrenWithRenderers(obj))
                {
                    gameObjectsToUpdate.Add(i);
                }
            }

            //ApplyOverrender(crewmates);
            ApplyOverrender(gameObjectsToUpdate);

            MelonLogger.Msg($"Enabled wallhacks for ${wallHackCount} objects");
        }



        private void InstanceChangedCheck()
        {
            if (newInstanceDetectorObject != null)
            {
                try
                {
                    string? name = newInstanceDetectorObject.name;
                    return;
                } catch
                {

                }
            }
            newInstanceDetectorObject = new GameObject();
            speedEnabled = false;
            collidersToggled = false;
            imposterEnabled = false;
            MelonLogger.Msg("Instance change detected! Resetting values.");
        }
    }
}
