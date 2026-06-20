using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }


    }

    private void Start()
    {
        // Inside any MonoBehaviour
        GameReference.UIAudioSource = GameReference.Player.AddComponent<AudioSource>();
        GameReference.BGMAudioSource = GameReference.Player.AddComponent<AudioSource>();
        GameReference.AmbienceAudioSource = GameReference.Player.AddComponent<AudioSource>();

        GameReference.AmbienceAudioSource.outputAudioMixerGroup = GameReference.AudioRegistry.audioMixer.FindMatchingGroups("Master/Ambience")[0];
        GameReference.AmbienceAudioSource.clip = GameReference.AudioRegistry.GetDefinition("CityAmbience").clip;
        GameReference.AmbienceAudioSource.Play();
        GameReference.BGMAudioSource.outputAudioMixerGroup = GameReference.AudioRegistry.audioMixer.FindMatchingGroups("Master/BGM")[0];
        GameReference.BGMAudioSource.clip = GameReference.AudioRegistry.GetDefinition("Holidays").clip;
        GameReference.BGMAudioSource.Play();

        GameReference.UIAudioSource.outputAudioMixerGroup = GameReference.AudioRegistry.audioMixer.FindMatchingGroups("Master/UI")[0];
    }
}
