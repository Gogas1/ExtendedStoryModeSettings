using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ExtendedStoryModeSettings;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class ExtendedStoryModeSettings : BaseUnityPlugin {
    private Harmony harmony = null!;

    private Slider? cachedAttackSlider = null;
    private Slider? cachedTakenDamageSlider = null;

    private void Awake() {
        Log.Init(Logger);
        RCGLifeCycle.DontDestroyForever(gameObject);
        harmony = Harmony.CreateAndPatchAll(typeof(ExtendedStoryModeSettings).Assembly);
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode) {        
        string attackSliderPath = $"Yee Attack Ratio";
        string takenDamageSliderPath = $"Yee Take Damage";

        if(cachedAttackSlider != null && cachedTakenDamageSlider != null) {
            return;
        }

        Slider[] allObjects = Object.FindObjectsOfType<Slider>(true);

        Slider attackRatioSlider = null!;
        Slider takenDamageSliderSlider = null!;

        foreach (Slider obj in allObjects) {
            if (obj.gameObject.scene.name == null || obj.gameObject.scene.name == "DontDestroyOnLoad") {
                if (obj.name == attackSliderPath) {
                    attackRatioSlider = obj;
                }
                if (obj.name == takenDamageSliderPath) {
                    takenDamageSliderSlider = obj;
                }
            }
        }

        cachedAttackSlider = attackRatioSlider;
        cachedTakenDamageSlider = takenDamageSliderSlider;

        if (attackRatioSlider != null) {
            attackRatioSlider.minValue = 0f;
            attackRatioSlider.OverrideStpSize = 1;

        }

        if (takenDamageSliderSlider != null) {
            takenDamageSliderSlider.maxValue = 1000f;
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneChanged;
        harmony.UnpatchSelf();
    }
}