using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    [Header("Registry References")]
    public SkyBoxWeatherRegistry weatherRegistry;
    public TimeOfDayRegistry timeRegistry;

    [Header("Settings")]
    public bool randomizeWeatherOnStart = true;
    public bool randomizeTimeOnStart = true;
    [Tooltip("Seconds to blend between skyboxes and lighting transitions.")]
    public float transitionDuration = 1f;
    [Tooltip("Parent transform for weather particle instances.")]
    public Transform particleParent;

    private WeatherDefinition currentWeather;
    private TimeOfDayDefinition currentTime;
    private GameObject activeWeatherParticles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }

    private void Start()
    {
        //if (particleParent == null)
            particleParent = GameReference.Player.transform;
        if (randomizeTimeOnStart && timeRegistry != null)
            SetTimeOfDay(timeRegistry.GetRandomTime());

        if (randomizeWeatherOnStart && weatherRegistry != null)
            SetWeather(weatherRegistry.GetWeatherByName("None"));
    }

    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    // WEATHER MANAGEMENT
    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    public void SetWeatherByName(string name)
    {
        if (weatherRegistry == null)
        {
            Debug.LogWarning("WeatherRegistry not assigned in EnvironmentManager.");
            return;
        }

        WeatherDefinition weather = weatherRegistry.GetWeatherByName(name);
        if (weather == null)
        {
            Debug.LogWarning($"Weather '{name}' not found in WeatherRegistry.");
            return;
        }

        SetWeather(weather);
    }

    public void SetWeather(WeatherDefinition weather)
    {
        if (weather == null) return;

        StopAllCoroutines();
        currentWeather = weather;

        // Fog
        RenderSettings.fog = weather.enableFog;
        RenderSettings.fogColor = weather.fogColor;
        RenderSettings.fogDensity = weather.fogDensity;

        // Weather Particles
        if (activeWeatherParticles != null)
            Destroy(activeWeatherParticles);

        if (weather.particlePrefab != null)
        {
            activeWeatherParticles = Instantiate(weather.particlePrefab, particleParent);
            activeWeatherParticles.transform.localPosition = new Vector3(0f, 5f, 0f);
        }

        // Ambient Sound
        if (weather.ambientSound != null)
        {
            GameReference.AmbienceAudioSource.clip = weather.ambientSound;
            if (!GameReference.AmbienceAudioSource.isPlaying)
                GameReference.AmbienceAudioSource.Play();
        }
        else
        {
            GameReference.AmbienceAudioSource.Stop();
            GameReference.AmbienceAudioSource.clip = null;
        }

        // Lighting intensity adjustment
        if (RenderSettings.sun != null)
            RenderSettings.sun.intensity = weather.lightIntensityMultiplier;

        DynamicGI.UpdateEnvironment();
        Debug.Log($"[EnvironmentManager] Weather changed to: {weather.weatherName}");
    }

    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    // TIME OF DAY MANAGEMENT
    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    public void SetTimeOfDayByName(string name)
    {
        if (timeRegistry == null)
        {
            Debug.LogWarning("TimeOfDayRegistry not assigned in EnvironmentManager.");
            return;
        }

        TimeOfDayDefinition time = timeRegistry.GetTimeByName(name);
        if (time == null)
        {
            Debug.LogWarning($"Time of day '{name}' not found in TimeOfDayRegistry.");
            return;
        }

        SetTimeOfDay(time);
    }

    public void SetTimeOfDay(TimeOfDayDefinition time)
    {
        if (time == null) return;

        StopAllCoroutines();
        currentTime = time;

        // Skybox transition
        if (time.skyboxMaterial != null)
            StartCoroutine(LerpSkybox(RenderSettings.skybox, time.skyboxMaterial, transitionDuration));

        if (time.backGroundMusic != null)
        {
            GameReference.BGMAudioSource.clip = time.backGroundMusic;
            if (!GameReference.BGMAudioSource.isPlaying)
                GameReference.BGMAudioSource.Play();
        }

        // Lighting & ambient
        if (RenderSettings.sun != null)
        {
            RenderSettings.sun.color = time.directionalLightColor;
            RenderSettings.sun.intensity = time.directionalLightIntensity;
        }

        RenderSettings.ambientLight = time.ambientColor;
        DynamicGI.UpdateEnvironment();

        Debug.Log($"[EnvironmentManager] Time of day changed to: {time.timeName}");
    }

    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    // SKYBOX TRANSITION
    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    private IEnumerator LerpSkybox(Material from, Material to, float duration)
    {
        if (to == null)
        {
            RenderSettings.skybox = from;
            yield break;
        }

        if (from == null)
        {
            RenderSettings.skybox = to;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            RenderSettings.skybox.Lerp(from, to, t / duration);
            yield return null;
        }

        RenderSettings.skybox = to;
        DynamicGI.UpdateEnvironment();
    }

    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    // HELPERS
    // ©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤
    public string GetCurrentWeatherName() => currentWeather ? currentWeather.weatherName : "None";
    public string GetCurrentTimeName() => currentTime ? currentTime.timeName : "None";
}
