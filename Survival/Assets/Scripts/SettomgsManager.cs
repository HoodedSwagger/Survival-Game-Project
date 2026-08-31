using UnityEngine;
using UnityEngine.Audio;

public class SettomgsManager : MonoBehaviour
{
    public static SettomgsManager Instance;

    [SerializeField] private AudioMixer mixer;

    private Resolution[] resolutions;
    
    private void Awake()
    {
        if(Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        resolutions = Screen.resolutions;
    }

    public void ApplySaveData(SettingsSaveData settingsSaveData)
    {
        
    }

    public void SetResolution()
    {

    }

    public void SetVolume()
    {

    }

    public void SetFullscreen()
    {

    }
    public void GetResolutionOptions()
    {

    }
}
