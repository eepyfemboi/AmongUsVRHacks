using MelonLoader;
using AmongUsHacks.Main;
using AmongUsHacks.Data;
using AmongUsHacks.Utils;
using System.Threading.Tasks;
using AmongUsHacks.Features;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Il2CppEasyRoads3Dv3;
using Il2CppSG.Airlock;
using UnityEngine.UI;
using UnityEngine.Events;
using Il2CppSystem.Data;
using static UnityEngine.GraphicsBuffer;
using Il2CppInterop.Runtime.InteropTypes;
using System.Reflection;


namespace AmongUsHacks.Main
{
    public class UIMenuOperator
    {
        private GameObject menuInstance;
        private Transform mainMenu, hostMenu, normalMenu, killAllMenu;

        private GameObject? menuPrefab = null;
        private GameObject? raycastPrefab = null;
        private AssetBundle? bundle = null;

        private GameObject raycastInstance;

        private VRPointerController pointerController;

        private void LoadMenu()
        {
            string bundlePath = Path.Combine("Mods", "eepyfemboi", "AdditionalAssets");
            MelonLogger.Msg("Loading AssetBundle from: " + bundlePath);

            if (bundle == null)
            {
                bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle == null)
                {
                    MelonLogger.Error("Failed to load AssetBundle.");
                    return;
                }
            }


            //bundle.LoadAllAssets();

            GameObject mprefab = bundle.LoadAsset<GameObject>("ModMenuUI");
            if (mprefab == null)
            {
                MelonLogger.Error("Failed to load prefab 'ModMenuUI' from AssetBundle.");
                return;
            }

            menuPrefab = mprefab;

            GameObject rprefab = bundle.LoadAsset<GameObject>("Raycast");
            if (rprefab == null)
            {
                MelonLogger.Error("Failed to load prefab 'ModMenuUI' from AssetBundle.");
                return;
            }

            raycastPrefab = rprefab;
        }

        public void EnsureSetToHand()
        {
            GameObject? lhand = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(go => go.name == "LeftHand");
            if (lhand == null)
            {
                MelonLogger.Error("Could not find left hand");
                return;
            }

            menuInstance.transform.SetParent(lhand.transform, false);
            //Vector3 mlocalPos = new Vector3(-0.2f, 0f, 0.2f);
            Vector3 mlocalPos = new Vector3(-0.2f, -0.0436f, -0.3f);
            //Vector3 mlocalRot = new Vector3(50f, 310f, 0f);
            Vector3 mlocalRot = new Vector3(51.1581f, 302.0844f, 7.7634f);
            Vector3 mlocalSca = new Vector3(0.0005f, 0.0005f, 0.0005f);
            menuInstance.transform.localPosition = mlocalPos;
            menuInstance.transform.localEulerAngles = mlocalRot;
            menuInstance.transform.localScale = mlocalSca;

            GameObject? rhand = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(go => go.name == "RightHand");
            if (rhand == null)
            {
                MelonLogger.Error("Could not find right hand");
                return;
            }

            raycastInstance.transform.SetParent(rhand.transform, false);
            Vector3 rlocalPos = new Vector3(-0.02f, -0.04f, 0f);
            //Vector3 rlocalRot = new Vector3(0f, 270f, 322f);
            Vector3 rlocalRot = new Vector3(33.3661f, 0, 0);
            Vector3 rlocalSca = new Vector3(1f, 1f, 1f);
            raycastInstance.transform.localPosition = rlocalPos;
            raycastInstance.transform.localEulerAngles = rlocalRot;
            raycastInstance.transform.localScale = rlocalSca;
        }

        public void OpenMenu()
        {
            if (menuPrefab == null || raycastPrefab == null)
            {
                LoadMenu();
            }

            menuInstance = GameObject.Instantiate(menuPrefab);
            //GameObject.DontDestroyOnLoad(menuInstance);
            menuInstance.SetActive(true);

            raycastInstance = GameObject.Instantiate(raycastPrefab);
            //GameObject.DontDestroyOnLoad(raycastInstance);

            ReplaceWithModVRPointerController(raycastInstance);

            raycastInstance.SetActive(true);

            EnsureSetToHand();

            SetupMenus();
            SetupButtons();
        }

        private void SetupMenus()
        {
            mainMenu = menuInstance.transform.Find("Canvas/Main");
            hostMenu = menuInstance.transform.Find("Canvas/Host");
            normalMenu = menuInstance.transform.Find("Canvas/Normal");
            killAllMenu = menuInstance.transform.Find("Canvas/KillAll");

            mainMenu.gameObject.SetActive(true);
            hostMenu.gameObject.SetActive(false);
            normalMenu.gameObject.SetActive(false);
            killAllMenu.gameObject.SetActive(false);
        }

        private void SetupButtons()
        {
            try { mainMenu.Find("OpenHost").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => OpenHostB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { mainMenu.Find("OpenNormal").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => OpenNormalB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { mainMenu.Find("Close").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => CloseMainMenuB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }

            try { hostMenu.Find("RainbowColors").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => RainbowColorsB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { hostMenu.Find("KillAll").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => OpenKillAllMenuB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { hostMenu.Find("ForceImposter").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => ForceImposterB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { hostMenu.Find("RevealImposter").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => RevealImposterB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { hostMenu.Find("RainbowColors (2)").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => RainbowColors2B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { hostMenu.Find("KillAll (2)").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll2B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { hostMenu.Find("Close").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => CloseHostMenuB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }

            try { killAllMenu.Find("KillAll1").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll1B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { killAllMenu.Find("KillAll2").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll2_2B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { killAllMenu.Find("KillAll3").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll3B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { killAllMenu.Find("KillAll4").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll4B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { killAllMenu.Find("KillAll5").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll5B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { killAllMenu.Find("KillAll6").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAll6B())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { killAllMenu.Find("Close").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => CloseKillAllMenuB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }

            try { normalMenu.Find("UnignoreGhosts").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => UnignoreGhostsB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { normalMenu.Find("Speed").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => SpeedB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { normalMenu.Find("NoClip").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => NoClipB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { normalMenu.Find("KillCooldown").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillCooldownB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { normalMenu.Find("WallHack").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => WallHackB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { normalMenu.Find("KillAll (1)").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => KillAllFromNormalB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
            try { normalMenu.Find("Close").GetComponent<Button>().onClick.AddListener((UnityAction)(System.Action)(() => CloseNormalMenuB())); } catch (Exception ex) { MelonLogger.Error($"failed to conv {ex}"); }
        }


        private void OpenHostB()
        {
            mainMenu.gameObject.SetActive(false);
            hostMenu.gameObject.SetActive(true);
        }

        private void OpenNormalB()
        {
            mainMenu.gameObject.SetActive(false);
            normalMenu.gameObject.SetActive(true);
        }

        private void CloseMainMenuB()
        {
            menuInstance.SetActive(false);
        }


        private void RainbowColorsB()
        {
            RainbowColors.Toggle();
        }
        private void ForceImposterB()
        {
            ForceImposter.Execute();
        }
        private void RevealImposterB()
        {
            ImposterReveal.Toggle();
        }
        private void RainbowColors2B() { }
        private void KillAll2B() { }

        private void OpenKillAllMenuB()
        {
            hostMenu.gameObject.SetActive(false);
            killAllMenu.gameObject.SetActive(true);
        }

        private void CloseHostMenuB()
        {
            hostMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }


        private void KillAll1B() { KillEveryone.TestKillEveryoneMethod1(); }
        private void KillAll2_2B() { KillEveryone.TestKillEveryoneMethod2(); }
        private void KillAll3B() { KillEveryone.TestKillEveryoneMethod3(); }
        private void KillAll4B() { KillEveryone.TestKillEveryoneMethod4(); }
        private void KillAll5B() { KillEveryone.TestKillEveryoneMethod5(); }
        private void KillAll6B() { KillEveryone.TestKillEveryoneMethod6(); }

        private void CloseKillAllMenuB()
        {
            killAllMenu.gameObject.SetActive(false);
            hostMenu.gameObject.SetActive(true);
        }


        private void UnignoreGhostsB()
        {
            UnignoreGhosts.Toggle();
        }
        private void SpeedB()
        {
            Speed.Toggle();
        }
        private void NoClipB()
        {
            Colliders.Toggle();
        }
        private void KillCooldownB()
        {
            KillCooldown.Toggle();
        }
        private void WallHackB()
        {
            Wallhack.Toggle();
        }
        private void KillAllFromNormalB() { }

        private void CloseNormalMenuB()
        {
            normalMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }


        public void ToggleMenu()
        {
            if (menuInstance == null)
            {
                OpenMenu();
            }
            EnsureSetToHand();
            menuInstance.SetActive(!menuInstance.activeSelf);
            raycastInstance.SetActive(!raycastInstance.activeSelf);
            Camera.main.cullingMask = -1;
        }


        /*private void ReplaceWithModVRPointerController(GameObject obj)
        {
            var existing = obj.GetComponent<MonoBehaviour>();
            if (existing == null)
            {
                MelonLogger.Msg("couldnt find it");
                return;
            }

            var newComponent = obj.AddComponent<VRPointerController>();

            var sourceType = existing.GetType();
            var targetType = typeof(VRPointerController);

            var fields = sourceType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                // Only copy public or [SerializeField] fields
                if (!field.IsPublic && field.GetCustomAttribute(typeof(UnityEngine.SerializeField), true) == null)
                    continue;

                var targetField = targetType.GetField(field.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (targetField != null && targetField.FieldType == field.FieldType)
                {
                    var value = field.GetValue(existing);
                    targetField.SetValue(newComponent, value);
                }
            }

            //GameObject.Destroy(existing);
            MelonLogger.Msg("Replaced bundled VRPointerController with mod version and copied values.");
        }*/
        private void ReplaceWithModVRPointerController(GameObject obj)
        {
            Component existing = null;

            foreach (var comp in obj.GetComponents<Component>())
            {
                if (comp == null) continue;
                if (comp.GetType().Name == "VRPointerController")
                {
                    existing = comp;
                    break;
                }
            }

            if (existing == null)
            {
                MelonLogger.Warning($"No baked VRPointerController found on {obj.name}.");
                //return;
            }

            var newComponent = obj.AddComponent<VRPointerController>();

            newComponent.maxRayDistance = 10f;
            newComponent.uiLayerMask = 1 << 5;
            newComponent.lineRenderer = obj.GetComponent<LineRenderer>();

            GameObject.DestroyImmediate(existing);
            MelonLogger.Msg("Simplified: Replaced baked VRPointerController with mod version.");
        }


    }
}
