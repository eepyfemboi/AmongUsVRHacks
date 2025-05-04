// this version of the file was made by https://github.com/AthenosTM

using UnityEngine;
using MelonLoader;
using AmongUsHacks.Data;
using AmongUsHacks.Main;
using Il2CppSG.Airlock.XR;
using Il2Cpp;
using Il2CppSG.Airlock.Blindbox;
using UnityEngine.SceneManagement;
using static Il2Cpp.Utils;
using System.Collections;

namespace AmongUsHacks.Features
{
    public static class Colliders
    {
        public static bool toggled = false;

        public static void Toggle()
        {
            MelonLogger.Msg($"Toggling NoClip..");

            if (!toggled)
                ReEnableNoClipping();
            else
                DisableNoClipping();

            toggled = !toggled;
            MelonLogger.Msg($"NoClip was successfuly toggled!");
        }

        private static void DisableNoClipping()
        {
            foreach (XRRig rig in GameObject.FindObjectsOfType<XRRig>())
            {
                rig._collider.enabled = true;
                rig.gameObject.GetComponent<Collider>().enabled = true;
                rig.gameObject.GetComponent<CapsuleCollider>().enabled = true;

                Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                string sceneName = currentScene.name;

                if (sceneName == "PolusPoint")
                {
                    GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

                    foreach (GameObject obj in allObjects)
                    {
                        if (obj.name.Contains("EntireLevelSightbox"))
                        {
                            obj.SetActive(false);
                        }
                    }
                }
                else
                {
                    GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(false);

                    foreach (GameObject obj in allObjects)
                    {
                        if (obj.name.ToLower().Contains("blind"))
                        {
                            obj.SetActive(true);
                        }
                    }
                }

                MelonLogger.Msg($"Disabled Player Collision!");
            }
        }

        private static void ReEnableNoClipping()
        {
            try
            {
                foreach (XRRig rig in GameObject.FindObjectsOfType<XRRig>())
                {
                    rig._collider.enabled = false;
                    rig.gameObject.GetComponent<Collider>().enabled = false;
                    rig.gameObject.GetComponent<CapsuleCollider>().enabled = false;

                    Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                    string sceneName = currentScene.name;

                    if (sceneName == "PolusPoint")
                    {
                        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

                        foreach (GameObject obj in allObjects)
                        {
                            if (obj.name.Contains("EntireLevelSightbox"))
                            {
                                // Start coroutine to enable after specified delay
                                MelonCoroutines.Start(EnableSightboxAfterDelay(obj));
                            }
                        }
                    }
                    else
                    {
                        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

                        foreach (GameObject obj in allObjects)
                        {
                            if (obj.name.ToLower().Contains("blind"))
                            {
                                obj.SetActive(false);
                            }
                        }
                    }

                    MelonLogger.Msg($"Enabled Player Collision");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"Exception while enabling NoClip: {ex}");
            }

            MelonLogger.Msg($"Restored XRRig Collision");
        }

        // Uses coroutine to forcefully enable the "EntireLevelSightbox". I've tried other ways, This shit is actually just the one way I could find over 2 hours of testing
        private static IEnumerator EnableSightboxAfterDelay(GameObject sightbox)
        {
            yield return new WaitForSeconds(0.2f);
            if (sightbox != null && !sightbox.activeSelf)
            {
                sightbox.SetActive(false); // Reset if required / needed
                yield return new WaitForSeconds(0.1f);
                sightbox.SetActive(true);  // Attempt to enable EntireLevelSightbox again
                //MelonLogger.Msg($"Forcefully disabled Blindboxes on Polus Point!");
            }
        }
    }
}