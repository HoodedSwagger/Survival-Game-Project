using UnityEngine;

[CreateAssetMenu(fileName = "SoundDefinition", menuName = "Scriptable Objects/SoundDefinition")]
public class SoundDefinition : ScriptableObject
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField, Range(0.8f, 1.2f)] private float pitchMin = 1f;
    [SerializeField, Range(0.8f, 1.2f)] private float pitchMax = 1f;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;
    public void Play(AudioSource source)
    {
        if (sounds.Length == 0) return;
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.volume = volume;
        source.Play();
    }
} 
