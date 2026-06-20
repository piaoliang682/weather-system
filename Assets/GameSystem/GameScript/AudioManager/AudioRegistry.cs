using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "AudioRegistry", menuName = "Audio/Registry", order = 2)]
public class AudioRegistry : ScriptableObject
{
    public AudioMixer audioMixer;
    [Header("Registered Audio Definitions")]
    public List<AudioDefinition> audioDefinitions = new List<AudioDefinition>();

    private Dictionary<string, AudioDefinition> lookupTable;

    /// <summary>
    /// Build dictionary for quick lookup.
    /// </summary>
    public void Initialize()
    {
        lookupTable = new Dictionary<string, AudioDefinition>();
        foreach (var def in audioDefinitions)
        {
            if (def != null && !lookupTable.ContainsKey(def.id))
            {
                lookupTable.Add(def.id, def);
            }
        }
    }

    /// <summary>
    /// Get an AudioDefinition by ID.
    /// </summary>
    public AudioDefinition GetDefinition(string id)
    {
        if (lookupTable == null)
            Initialize();

        lookupTable.TryGetValue(id, out AudioDefinition def);
        if (def == null) Debug.LogWarning($"{id} not found");
        return def;
    }
}
