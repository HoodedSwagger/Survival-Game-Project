using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SettingsPanelUI : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider effectsVolume;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    public void Start()
    {
        SettingsManager.Instance.InitUI(masterVolume,effectsVolume,resolutionDropdown);
    }

    private void OnDestroy()
    {
        SettingsManager.Instance?.DeInitUI();
    }
}
