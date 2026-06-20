using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SkyBoxWeatherRegistry",
    menuName = "Game/Weather Registry",
    order = 1)]
public class SkyBoxWeatherRegistry : ScriptableObject
{
    [Tooltip("List of all available weather profiles.")]
    public List<WeatherDefinition> weatherProfiles = new List<WeatherDefinition>();

    private Dictionary<string, WeatherDefinition> weatherLookup;

    /// <summary>
    /// Initializes the internal lookup dictionary.
    /// </summary>
    private void OnEnable()
    {
        BuildLookup();
    }

    /// <summary>
    /// Builds a dictionary for quick access by name.
    /// </summary>
    public void BuildLookup()
    {
        weatherLookup = new Dictionary<string, WeatherDefinition>();

        foreach (var profile in weatherProfiles)
        {
            if (profile == null || string.IsNullOrEmpty(profile.weatherName))
                continue;

            if (!weatherLookup.ContainsKey(profile.weatherName))
                weatherLookup.Add(profile.weatherName, profile);
            else
                Debug.LogWarning($"Duplicate weather name detected: {profile.weatherName}");
        }
    }

    /// <summary>
    /// Gets a weather profile by its name.
    /// </summary>
    public WeatherDefinition GetWeatherByName(string name)
    {
        if (weatherLookup == null || weatherLookup.Count == 0)
            BuildLookup();

        if (weatherLookup.TryGetValue(name, out var profile))
            return profile;

        Debug.LogWarning($"Weather '{name}' not found in registry.");
        return null;
    }

    /// <summary>
    /// Returns a random weather profile.
    /// </summary>
    public WeatherDefinition GetRandomWeather()
    {
        if (weatherProfiles == null || weatherProfiles.Count == 0)
            return null;

        return weatherProfiles[Random.Range(0, weatherProfiles.Count)];
    }
}
