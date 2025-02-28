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

        private MelonPreferences_Entry<KeyCode>? wallHackToggleKey_config;


        private KeyCode imposterToggleKey = KeyCode.I;
        private bool imposterEnabled = false;

        private MelonPreferences_Entry<KeyCode>? imposterToggleKey_config;


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

            imposterToggleKey_config = settings.CreateEntry("ImposterToggleKey", imposterToggleKey, "Toggle Show Imposters Keybind");


            remoteBlacklistURL = remoteBlacklistURL_config.Value;
            doRemoteBlacklistUpdate = doRemoteBlacklistUpdate_config.Value;

            collidersToggleKey = collidersToggleKey_config.Value;

            speedToggleKey = speedToggleKey_config.Value;
            speedSetting = speedSetting_config.Value;

            wallHackToggleKey = wallHackToggleKey_config.Value;

            imposterToggleKey = imposterToggleKey_config.Value;
        }

        public override void OnUpdate()
        {
            HandleKeybinds();
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
    }
}
