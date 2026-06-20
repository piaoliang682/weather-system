using UnityEngine;

[CreateAssetMenu(
    fileName = "WeatherDefinition",
    menuName = "Environment/Weather Definition",
    order = 0)]
public class WeatherDefinition : ScriptableObject
{
    [Header("General")]
    public string weatherName;

    [Header("Fog Settings")]
    public bool enableFog = true;
    public Color fogColor = Color.gray;
    [Range(0f, 0.1f)] public float fogDensity = 0.01f;

    [Header("Weather Effects")]
    [Tooltip("Prefab for weather particles (rain, snow, etc.)")]
    public GameObject particlePrefab;

    [Tooltip("Ambient weather sound (rain, wind, etc.)")]
    public AudioClip ambientSound;

    [Header("Other Settings")]
    [Tooltip("Multiplier for light intensity under this weather.")]
    [Range(0f, 1f)] public float lightIntensityMultiplier = 1f;
}
