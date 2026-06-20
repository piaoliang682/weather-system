using UnityEngine;

[CreateAssetMenu(
    fileName = "TimeOfDayDefinition",
    menuName = "Environment/Time of Day Definition",
    order = 1)]
public class TimeOfDayDefinition : ScriptableObject
{
    [Header("Time of Day")]
    public string timeName; // e.g. Morning, Noon, Evening, Night

    [Header("Sky")]
    public Material skyboxMaterial;

    [Header("Lighting")]
    public Color directionalLightColor = Color.white;
    [Range(0f, 10f)] public float directionalLightIntensity = 1f;
    [Tooltip("BGM")]
    public AudioClip backGroundMusic;
    [Header("Ambient Light")]
    public Color ambientColor = Color.gray;
    [Range(0f, 1f)] public float ambientIntensity = 1f;
}
