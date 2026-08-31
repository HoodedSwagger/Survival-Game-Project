using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }
    public void Play(SoundDefinition sound)
    {
        if (sound == null) return;
        sound.Play(audioSource);
    }
}
