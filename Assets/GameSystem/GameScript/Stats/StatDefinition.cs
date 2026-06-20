using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stat Definition", fileName = "NewStatDefinition")]
public class StatDefinition : ScriptableObject
{
    [Header("General Info")]
    public StatType statType;
    public string displayName;
    [TextArea] public string description;

    [Header("UI Display")]
    public Color displayColor = Color.white;
    public Sprite icon;

    [Header("Default Values")]
    public float defaultValue = 50f;
    public float minValue = 0f;
    public float maxValue = 100f;
}
