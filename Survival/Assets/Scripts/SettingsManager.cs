using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private TMP_Dropdown _resolutionDropdown;
    private Slider _masterSlider;
    private Slider _effectsSlider;

    [SerializeField] private AudioMixer mixer;

    private Resolution[] resolutions;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        resolutions = Screen.resolutions;
    }

    public void InitUI(Slider masterSlider, Slider effectsSlider, TMP_Dropdown resolutionDropdown)
    {
        _masterSlider = masterSlider;
        _effectsSlider = effectsSlider;
        _resolutionDropdown = resolutionDropdown;

        mixer.GetFloat("Master", out float masterDb);
        masterSlider.value = Mathf.Pow(10f, masterDb / 20f);
        mixer.GetFloat("Effects", out float effectsDb);
        effectsSlider.value = Mathf.Pow(10f, effectsDb / 20f);

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(new List<string>(GetResolutionOptions()));
        _resolutionDropdown.value = GetCurrentResolutionIndex();
        _resolutionDropdown.RefreshShownValue();

        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
    }
    public void DeInitUI()
    {
        if (_masterSlider != null) _masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        if (_effectsSlider != null) _effectsSlider.onValueChanged.RemoveListener(SetEffectsVolume);
        if (_resolutionDropdown != null) _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionDropdownChanged);

        _masterSlider = null;
        _effectsSlider = null;
        _resolutionDropdown = null;
    }

    public void ApplySaveData(SettingsSaveData settingsSaveData)
    {
        mixer.SetFloat("Master", ConvertToDecibels(settingsSaveData.masterVolume));
        mixer.SetFloat("Effects", ConvertToDecibels(settingsSaveData.effectsVolume));

        var r = resolutions[settingsSaveData.resolutionIndex];
        SetResolution(r.width, r.height, 
            settingsSaveData.isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }
    public SettingsSaveData GetSaveData()
    {
        SettingsSaveData settingsSave = new SettingsSaveData();

        mixer.GetFloat("Master", out float masterDb);
        settingsSave.masterVolume = Mathf.Pow(10, masterDb / 20f);
        mixer.GetFloat("Effects", out float effectsDb);
        settingsSave.effectsVolume = Mathf.Pow(10f, effectsDb / 20f);

        settingsSave.isFullscreen = Screen.fullScreen;

        settingsSave.resolutionIndex = GetCurrentResolutionIndex();

        return settingsSave;
    }

    public void SetResolution(int width, int height, FullScreenMode fullScreenMode)
    {
        Screen.SetResolution(width, height, fullScreenMode);
    }
    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }
    public void SetMasterVolume(float value) => SetVolume("Master", value);
    public void SetEffectsVolume(float value) => SetVolume("Effects", value);

    public void OnResolutionDropdownChanged(int index)
    {
        var r = resolutions[index];
        Screen.SetResolution(r.width,r.height,Screen.fullScreenMode);
    }

    private void SetVolume(string name, float value )
    {
        mixer.SetFloat(name, ConvertToDecibels(value));
    }

    private string[] GetResolutionOptions()
    {
        var options = new string[resolutions.Length];

        for (int i = 0; i < resolutions.Length; i++)
        {
            var r = resolutions[i];
            options[i] = $"{r.width}x{r.height} {r.refreshRateRatio.value:F0}Hz";
        }
        return options;
    }
    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height) 
                return i;
        }
        return resolutions.Length - 1;
    }

    private float ConvertToDecibels(float value)
    {
        float db = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        return db;
    }
}
