using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private SoundDefinition soundDef;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    public void Play()
    {
        soundDef.Play(audioSource);
    }
}
