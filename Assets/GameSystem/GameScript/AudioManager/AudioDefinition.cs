using UnityEngine;

[CreateAssetMenu(fileName = "AudioDefinition", menuName = "Audio/Definition", order = 1)]
public class AudioDefinition : ScriptableObject
{
    [Header("Audio Clip Settings")]
    public string id;                     // Unique ID to identify the sound
    public AudioClip clip;                // The actual audio clip

    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    public bool loop = false;

    [Header("Category")]
    public AudioCategory category = AudioCategory.SFX;  // Enum for grouping

    public enum AudioCategory
    {
        BGM,
        Ambience,
        SFX,
        UI,
        Voice
    }
}
