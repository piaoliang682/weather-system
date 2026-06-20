using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Mixer Parameter Names")]
    public string masterVolumeParam = "MasterVolume";
    public string musicVolumeParam = "BGMVolume";
    public string sfxVolumeParam = "SFXVolume";

    [Header("Optional UI Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Optional Mute Toggles")]
    public Toggle musicMuteToggle;
    public Toggle sfxMuteToggle;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    void Start()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        SetMasterVolume(savedMasterVolume);
        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        // Set slider values first
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = savedMasterVolume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = savedMusicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = savedSFXVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Then set mute toggles ¡ª avoid slider.value overwrite
        if (musicMuteToggle != null)
        {
            musicMuteToggle.isOn = savedMusicVolume <= 0.0001f;
            musicMuteToggle.onValueChanged.AddListener((muted) =>
            {
                float volume = muted ? 0.0001f : (musicVolumeSlider != null ? musicVolumeSlider.value : 1f);
                SetMusicVolume(volume);
            });
        }

        if (sfxMuteToggle != null)
        {
            sfxMuteToggle.isOn = savedSFXVolume <= 0.0001f;
            sfxMuteToggle.onValueChanged.AddListener((muted) =>
            {
                float volume = muted ? 0.0001f : (sfxVolumeSlider != null ? sfxVolumeSlider.value : 1f);
                SetSFXVolume(volume);
            });
        }
    }


    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(masterVolumeParam, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeParam, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolumeParam, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    public void ResetAudioSettings()
    {
        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);

        if (masterVolumeSlider != null) masterVolumeSlider.value = 1f;
        if (musicVolumeSlider != null) musicVolumeSlider.value = 1f;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = 1f;

        if (musicMuteToggle != null) musicMuteToggle.isOn = false;
        if (sfxMuteToggle != null) sfxMuteToggle.isOn = false;

        PlayerPrefs.DeleteKey(MasterVolumeKey);
        PlayerPrefs.DeleteKey(MusicVolumeKey);
        PlayerPrefs.DeleteKey(SFXVolumeKey);
    }
}
