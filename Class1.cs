using MelonLoader;
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
using Il2CppSG.Airlock.Minigames;
using Il2CppSG.Airlock.Minimap;
using Il2CppSG.Airlock.Blindbox;
using Il2CppSG.Airlock.Doors;
using Il2CppSG.Airlock.GlobalEvents;
using Il2CppSG.Airlock.Sabotage;
using Il2CppSG.Airlock.Settings;
using Il2CppSG.Airlock.Utilities;
using Il2CppSG.Airlock.Utility;
using Il2CppSG.Airlock.Venting;
using Il2CppSG.GlobalEvents.Events;
using System.Runtime.CompilerServices;
using Il2CppSG.Airlock.UI.DebugMenu;
using System;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using HarmonyLib;
using Il2CppPhoton.Voice.Fusion;
using Il2CppSG.Platform.Steam;
using Il2CppSG.Platform;
using Il2CppSG.PlayerLoops;
using Il2CppSteamworks;
using Il2CppSG.SkuBuild.Steam;
using Il2CppValve.VR;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace AmongUsHacks
{
    public class Class1 : MelonMod
    {
        private MelonPreferences_Category? settings;


        private bool blacklistUpdated = false;
        private string remoteBlacklistURL = "https://sleepie.dev/amongusvr/mod/dynamic_data/blacklisted_object_names.txt";
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



        private KeyCode tempForceCompleteTaskKey = KeyCode.LeftBracket;
        private KeyCode tempForceCompleteMinigameKey = KeyCode.RightBracket;
        private KeyCode tempForceCompleteTaskViaInvokeKey = KeyCode.Semicolon;


        private bool killCoolDownActive = false;
        private KeyCode tempForceKillCooldownLowerKey = KeyCode.K;
        NetworkedKillBehaviour? killManager = null;

        private KeyCode tempForceImposterKeyCode = KeyCode.F;

        private bool forceUnignoreGhosts = false;
        private KeyCode forceUnignoreGhostsKey = KeyCode.U;
        private int forceUnignoreGhostsFrame = 0;
        private int forceUnignoreGhostsUpdateFrameInterval = 100; // for anyone wondering why im doing it this way, its bcuz i dont wanna impact the games performance by running a potentially straining function every frame, so i'll do it every 100 frames instead


        private KeyCode testKillEveryoneActivatorKey = KeyCode.DownArrow;
        private bool testKillEveryonePrimed = false;
        private KeyCode testKillEveryoneUseSelfKey = KeyCode.UpArrow;
        private bool testKillEveryoneUseSelf = false;



        private static readonly string VersionUrl = "https://sleepie.dev/amongusvr/mod/version";
        private static readonly string DownloadUrl = "https://sleepie.dev/amongusvr/mod/latest/AmongUsHacks.dll";
        private static readonly string ModFileName = "AmongUsHacks.dll";
        private static readonly string ModFolderPath = "Mods";

        private static readonly string CurrentVersion = "1.0.1";
        private static readonly string UserAgent = "AmongUsVRHacks/1.0 (Windows; MelonLoader)";



        private static readonly System.Uri RPCWebSocketURI = new System.Uri("wss://amongusvr.sleepie.dev:443/rpc");
        private ClientWebSocket? webSocket;
        private CancellationTokenSource? cancellationTokenSource;
        private Task? receiveTask;



        private GameObject? newInstanceDetectorObject;








        private async Task CheckForUpdates()
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

                string latestVersion = await client.GetStringAsync(VersionUrl);
                latestVersion = latestVersion.Trim();

                if (latestVersion != CurrentVersion)
                {
                    MelonLogger.Msg($"New version available: {latestVersion} (Current: {CurrentVersion})");
                    await DownloadAndUpdate();
                }
                else
                {
                    MelonLogger.Msg("You are running the latest version.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Update check failed: {ex.Message}");
            }
        }

        private async Task DownloadAndUpdate()
        {
            try
            {
                string modFilePath = Path.Combine(ModFolderPath, ModFileName);
                string tempFilePath = modFilePath + ".new";
                string oldFilePath = modFilePath + ".old";
                string batchFilePath = Path.Combine(ModFolderPath, "UpdateMod.bat");

                MelonLogger.Msg("Downloading new mod version...");
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
                byte[] data = await client.GetByteArrayAsync(DownloadUrl);
                await File.WriteAllBytesAsync(tempFilePath, data);

                File.WriteAllText(batchFilePath, $@"
@echo off
:retry
timeout /t 3 >nul
del ""{oldFilePath}""
move ""{modFilePath}"" ""{oldFilePath}""
move ""{tempFilePath}"" ""{modFilePath}""
del ""{batchFilePath}""
start """" ""{Process.GetCurrentProcess().MainModule.FileName}""
exit
");

                MelonLogger.Msg("Update downloaded! Restarting game to apply changes...");
                RestartGameWithBatch(batchFilePath);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Update failed: {ex.Message}");
            }
        }

        private void RestartGameWithBatch(string batchFilePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = batchFilePath,
                    UseShellExecute = true,
                    CreateNoWindow = true
                });

                MelonLogger.Msg("Game restarting...");
                Application.Quit();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Failed to restart: {ex.Message}");
            }
        }




        private async void ConnectWebSocket()
        {
            try
            {
                webSocket = new ClientWebSocket();
                cancellationTokenSource = new CancellationTokenSource();

                webSocket.Options.SetRequestHeader("User-Agent", UserAgent);

                await webSocket.ConnectAsync(RPCWebSocketURI, CancellationToken.None);
                MelonLogger.Msg("Connected to RPC WebSocket!");

                receiveTask = ReceiveMessages();
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"RPC WebSocket connection error: {ex.Message}");
            }
        }

        private async Task ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new System.ArraySegment<byte>(buffer), cancellationTokenSource.Token);
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    MelonLogger.Msg($"Received: {message}");
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"RPC WebSocket receive error: {ex.Message}");
                    break;
                }
            }
            Reconnect();
        }

        private async void Reconnect()
        {
            MelonLogger.Warning("Reconnecting to RPC WebSocket...");
            await Task.Delay(5000);
            ConnectWebSocket();
        }






        public override void OnInitializeMelon()
        {
            LoadConfig();
            Task.Run(CheckForUpdates);

            MelonLogger.Msg("Sleepy's AmongUsVR Hacks Loaded! View the source code at https://github.com/eepyfemboi/AmongUsVRHacks");
            LoadBlacklist();

            ConnectWebSocket();
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


            //remoteBlacklistURL = remoteBlacklistURL_config.Value;
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
            if (killCoolDownActive)
            {
                DoKillCooldownThingie();
            }
            if (forceUnignoreGhosts)
            {
                DoForceUnignoreGhostsThingie();
            }
            if (testKillEveryonePrimed)
            {
                DoKillEveryoneTestThingie();
            }
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
            if (Input.GetKeyDown(tempForceCompleteTaskKey))
            {
                DoForceCompleteTask();
            }
            if (Input.GetKeyDown(tempForceCompleteMinigameKey))
            {
                DoForceCompleteMinigame();
            }
            if (Input.GetKeyDown(tempForceCompleteTaskViaInvokeKey))
            {
                DoForceCompleteViaInvoke();
            }
            if (Input.GetKeyDown(tempForceKillCooldownLowerKey))
            {
                //ReduceKillCooldown();
                ToggleKillCooldown();
            }
            if (Input.GetKeyDown(tempForceImposterKeyCode))
            {
                TestForceImposter();
                //ToggleForceImposter
            }
            if (Input.GetKeyDown(forceUnignoreGhostsKey))
            {
                ToggleForceUnignoreGhosts();
            }
            if (Input.GetKeyDown(testKillEveryoneActivatorKey))
            {
                //DoKillEveryoneTestThingie();
                testKillEveryonePrimed = !testKillEveryonePrimed;
                MelonLogger.Msg($"Toggled kill everyone primer to ${testKillEveryonePrimed}");
            }
            if (Input.GetKeyDown(testKillEveryoneUseSelfKey))
            {
                testKillEveryoneUseSelf = !testKillEveryoneUseSelf;
                MelonLogger.Msg($"toggled kill everyone use self to ${testKillEveryoneUseSelf}");
            }
        }
        

        private void DoForceUnignoreGhostsThingie()
        {
            if (forceUnignoreGhostsFrame > forceUnignoreGhostsUpdateFrameInterval)
            {
                forceUnignoreGhostsFrame = 0;
                UnignoreGhosts();
            } else
            {
                forceUnignoreGhostsFrame++;
            }
        }


        private void ToggleForceUnignoreGhosts()
        {
            MelonLogger.Msg($"Toggling force unignore ghosts to {!forceUnignoreGhosts}");

            forceUnignoreGhosts = !forceUnignoreGhosts;

            MelonLogger.Msg($"Toggled force unignore ghosts to {forceUnignoreGhosts}");
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

        private void ToggleKillCooldown()
        {
            MelonLogger.Msg($"Toggling kill cooldown to {!killCoolDownActive}");

            killCoolDownActive = !killCoolDownActive;

            MelonLogger.Msg($"Toggled kill cooldown to {killCoolDownActive}");
        }


        private void RefreshKillManager()
        {
            if (killManager == null)
            {
                killManager = GameObject.FindObjectOfType<NetworkedKillBehaviour>();
            }
        }

        private void DoKillCooldownThingie()
        {
            RefreshKillManager();

            if (killManager != null && killManager._killCooldown > 1)
            {
                killManager._killCooldown = 0.1f;
                MelonLogger.Msg("changed kill cooldown");
            }
        }



        private void FetchBlacklist()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
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
            foreach (NetworkedLocomotionPlayer player in GetNetworkedLocomotionPlayers())
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
                foreach (NetworkedLocomotionPlayer player in GetNetworkedLocomotionPlayers())
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

        private System.Collections.Generic.List<NetworkedLocomotionPlayer> GetRealPlayers()
        {
            System.Collections.Generic.List<NetworkedLocomotionPlayer> realPlayers = new System.Collections.Generic.List<NetworkedLocomotionPlayer>();

            foreach (NetworkedLocomotionPlayer player in GetNetworkedLocomotionPlayers())
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
            return realPlayers;
        }

        private int GetSelfPlayerID()
        {
            int playerId = -1;

            System.Collections.Generic.List<NetworkedLocomotionPlayer> realPlayers = GetRealPlayers();

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



        private System.Collections.Generic.List<NetworkedLocomotionPlayer> GetNetworkedLocomotionPlayers()
        {
            System.Collections.Generic.List<NetworkedLocomotionPlayer> players = new System.Collections.Generic.List<NetworkedLocomotionPlayer>();

            foreach (NetworkedLocomotionPlayer player in GameObject.FindObjectsOfType<NetworkedLocomotionPlayer>())
            {
                players.Add(player);
            }

            return players;
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
            if (speedEnabled)
            {
                speedEnabled = false;
                TogglePlayerSpeed();
            }
            if (collidersToggled)
            {
                collidersToggled = false;
                ToggleColliders();
            }
            if (imposterEnabled)
            {
                imposterEnabled = false;
                ToggleShowImposters();
            }
            if (wallHackEnabled)
            {
                wallHackEnabled = false;
                ToggleWallHack();
            }
            /*speedEnabled = false;
            collidersToggled = false;
            imposterEnabled = false;
            wallHackEnabled = false;*/
            MelonLogger.Msg("Instance change detected! Resetting values.");
        }

        private System.Collections.Generic.List<MinigameConsole> GetUserTasks()
        {
            System.Collections.Generic.List<MinigameConsole> tasks = new System.Collections.Generic.List<MinigameConsole>();

            foreach (MinigameConsole obj in GameObject.FindObjectsOfType<MinigameConsole>(true))
            {
                if (obj.gameObject.activeSelf)
                {
                    tasks.Add(obj);
                }
            }

            return tasks;
        }

        private void DoForceCompleteTask()
        {
            System.Collections.Generic.List<MinigameConsole> tasks = GetUserTasks();
            int forced = 0;

            foreach (MinigameConsole obj in tasks)
            {
                obj.ForceCompleteTask();
                forced++;
            }

            MelonLogger.Msg($"force completed {forced} tasks");
        }

        private void DoForceCompleteMinigame()
        {
            System.Collections.Generic.List<MinigameConsole> tasks = GetUserTasks();
            int forced = 0;

            foreach (MinigameConsole obj in tasks)
            {
                obj.ForceCompleteMinigame();
                forced++;
            }

            MelonLogger.Msg($"force completed {forced} minigames");
        }

        private void DoForceCompleteViaInvoke()
        {
            System.Collections.Generic.List<MinigameConsole> tasks = GetUserTasks();
            int forced = 0;

            foreach (MinigameConsole obj in tasks)
            {
                obj.OnComplete.Invoke();
                forced++;
            }

            MelonLogger.Msg($"force completed {forced} minigames");
        }


        private void ReduceKillCooldown()
        {
            NetworkedKillBehaviour killManager = GameObject.FindObjectOfType<NetworkedKillBehaviour>();
            killManager._killCooldown = 1;

            //killManager.SetMaxCooldown(1);
        }

        private GameObject? GetPlayerGhostObject(GameObject player)
        {
            foreach (Transform transform in player.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject.name.Contains("ghost", System.StringComparison.OrdinalIgnoreCase))
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        private GameObject? GetPlayerUINameTag(GameObject player)
        {
            foreach (Transform transform in player.GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject.name.Contains("ui", System.StringComparison.OrdinalIgnoreCase) && transform.gameObject.name.Contains("nametag", System.StringComparison.OrdinalIgnoreCase))
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        private void SwitchPlayerToAliveVoiceChatGroup(GameObject player)
        {
            VoiceChatManager manager = UnityEngine.Object.FindObjectOfType<VoiceChatManager>();
            foreach (VoiceChatActivator vcAct in player.GetComponentsInChildren<VoiceChatActivator>())
            {
                AudioSource audioSource = vcAct.gameObject.GetComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = manager._aliveVoiceChatGroup;
            }
        }

        private void UnignorePlayer(NetworkedLocomotionPlayer player)
        {
            GameObject? playerGhostObj = GetPlayerGhostObject(player.gameObject);
            if (playerGhostObj != null)
            {
                foreach (SkinnedMeshRenderer renderer in playerGhostObj.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    renderer.enabled = true;
                }
            }
            GameObject? nameTag = GetPlayerUINameTag(player.gameObject);
            if (nameTag != null)
            {
                nameTag.SetActive(true);
            }
            SwitchPlayerToAliveVoiceChatGroup(player.gameObject);
        }

        private void UnignoreGhosts()
        {
            System.Collections.Generic.List<NetworkedLocomotionPlayer> players = GetNetworkedLocomotionPlayers();
            int selfPlayerId = GetSelfPlayerID();
            foreach (NetworkedLocomotionPlayer player in GetRealPlayers())
            {
                if (player._cachedPlayerID != selfPlayerId)
                {
                    UnignorePlayer(player);
                    MelonLogger.Msg($"Unignoring player: {player._nameTag._storedName}");
                }
            }
        }


        private PlayerRef GetSelfPlayerRef()
        {
            int playerSelfId = GetSelfPlayerID();
            PlayerRef playerRef = (PlayerRef)playerSelfId;//new PlayerRef();
            return playerRef;
            //playerRef.PlayerId = playerSelfId;

            //System.Collections.Generic.List<PlayerRef> players = new System.Collections.Generic.List<PlayerRef>();

            // Find all objects of type PlayerRef in the scene
            //foreach (PlayerRef player in Object.FindObjectsOfType<PlayerRef>())
            //{
            //    players.Add(player);
            //}

            //return players;
            //PlayerRef playerRef;

            //playerRef.
            //playerRef.PlayerId
        }

        private void TestKillEveryoneMethod1()
        {
            RefreshKillManager();

            System.Collections.Generic.List<AirlockPeer> peers = new System.Collections.Generic.List<AirlockPeer>();
            System.Collections.Generic.List<PlayerState> players = new System.Collections.Generic.List<PlayerState>();
            //PlayerState
            //killManager.killpl

            foreach (AirlockPeer peer in UnityEngine.Object.FindObjectsOfType<AirlockPeer>())
            {
                peers.Add(peer);
                MelonLogger.Msg($"Adding peer: {peer.PeerID}");
            }
            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                players.Add(player);
                MelonLogger.Msg($"Adding player: {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }

            PlayerRef selfPlayerRef = GetSelfPlayerRef();

            if (peers.Count == players.Count)
            {
                foreach (PlayerState player in players)
                {
                    foreach (AirlockPeer peer in peers)
                    {
                        if (player.PlayerId == peer.PeerID)
                        {
                            killManager.KillPlayer(peer, player, selfPlayerRef, false);
                            MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
                        }
                    }
                }
            } else
            {
                foreach (PlayerState player in players)
                {
                    foreach (AirlockPeer peer in peers)
                    {
                        killManager.KillPlayer(peer, player, selfPlayerRef, false);
                        MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
                    }
                }
            }
        }

        private void TestKillEveryoneMethod2()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (testKillEveryoneUseSelf)
            {
                selfPlayerRef.value = GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(true, true, true, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        private void TestKillEveryoneMethod3()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (testKillEveryoneUseSelf)
            {
                selfPlayerRef.value = GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(true, false, true, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        private void TestKillEveryoneMethod4()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (testKillEveryoneUseSelf)
            {
                selfPlayerRef.value = GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(false, true, true, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        private void TestKillEveryoneMethod5()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (testKillEveryoneUseSelf)
            {
                selfPlayerRef.value = GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(true, true, false, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        private void TestKillEveryoneMethod6()
        {
            Il2CppSystem.Nullable<PlayerRef> selfPlayerRef = new Il2CppSystem.Nullable<PlayerRef>();
            if (testKillEveryoneUseSelf)
            {
                selfPlayerRef.value = GetSelfPlayerRef();
            }

            foreach (PlayerState player in UnityEngine.Object.FindObjectsOfType<PlayerState>())
            {
                player.KillPlayer(false, false, false, selfPlayerRef, false);
                MelonLogger.Msg($"Killed {player.AcceptedName} {player.CachedName} {player.PlayerId}");
            }
        }

        private void DoKillEveryoneTestThingie()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Keypad1))
            {
                TestKillEveryoneMethod1();
            } else if (Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Keypad2))
            {
                TestKillEveryoneMethod2();
            } else if (Input.GetKeyDown(KeyCode.Alpha3) | Input.GetKeyDown(KeyCode.Keypad3))
            {
                TestKillEveryoneMethod3();
            } else if (Input.GetKeyDown(KeyCode.Alpha4) | Input.GetKeyDown(KeyCode.Keypad4))
            {
                TestKillEveryoneMethod4();
            } else if (Input.GetKeyDown(KeyCode.Alpha5) | Input.GetKeyDown(KeyCode.Keypad5))
            {
                TestKillEveryoneMethod5();
            } else if (Input.GetKeyDown(KeyCode.Alpha6) | Input.GetKeyDown(KeyCode.Keypad6))
            {
                TestKillEveryoneMethod6();
            }
        }


        private void TestForceImposter()
        {

            NetworkedKillBehaviour killManager = GameObject.FindObjectOfType<NetworkedKillBehaviour>();
            PlayerRef playerRef = GetSelfPlayerRef();
            killManager.AlterRole(GameRole.Imposter, playerRef);
        }

        /*[HarmonyPatch(typeof(SteamFriends), "GetPersonaName")]
        public class Patch_GetPersonaName
        {
            public static bool Prefix(ref string __result)
            {
                string fakeUsername = "test1";
                __result = fakeUsername;
                MelonLogger.Msg($"Spoofing Steam Username: {fakeUsername}");
                return false;
            }
        }*/


        private void TestingThingie()
        {
            MinigamePlayer e1 = new MinigamePlayer();
            e1.BeginTask();
            MinigameManager e2 = new MinigameManager();
            //e2.AssignedTasks
            //e2.task
            //e1.ForceCompleteTask()
            //e2._minigames
            //e2.gameUIDToConsole
            MinigameConsole e3 = new MinigameConsole();
            e3.ForceCompleteMinigame();
            e3.ForceCompleteTask();
            //e3.OnComplete.Invoke()
            NetworkedLocomotionPlayer e4 = new NetworkedLocomotionPlayer();
            //e4.OnIsAliveChange
            NetworkedKillBehaviour e5 = new NetworkedKillBehaviour();
            //e5.KillPlayer
            //e5.
            //e4.TaskPlayer.AssignedTasks
            //e4._playerState._IsAlive
            //PlayerState
            VoiceChatManager e6 = new VoiceChatManager();
            //e6._voiceBridgePrefab
            AirlockVoiceBridge e7 = new AirlockVoiceBridge();
            //e7.
            /* i wanna work on a ban bypasser and name spoofer so im experimenting with steam stuff here lol */
            //SteamEntitlement e8 = new SteamEntitlement();
            //e8.
            //SteamAPI
            //SteamClient
            //SteamDLCAddOns
            //SteamManager
            //SteamNetworking
            //SteamNetworkingIdentity
            //SteamNetworkingUtils
            //SteamPhotonAuthentication
            //SteamSku
            //SteamUser
            //SteamUtils
            //SteamVR_Utils
            //SteamVR
            //SteamVR_Action
            //SteamVR_Utils
            //CSteamAPIContext
            //CSteamID

            


            //HSteamUser e8 = Il2CppSteamworks.SteamAPI.GetHSteamUser();
            //e8.m_HSteamUser.
            //SteamAPI.GetHSteamPipe().m_HSteamPipe.
        }
    }
}
