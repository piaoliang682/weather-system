using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PrefabVarientGroup", menuName = "Game/Prefab Variant Data")]
public class PrefabVariantGroupSO : ScriptableObject
{
    [Header("Basic Info")]
    public string varientGroupName;     // Name or ID (e.g. "Hair", "Clothes", "Expression")

    [Header("Default Sprite")]
    public GameObject defaultPrefab;   // The base or fallback sprite

    [Header("All Variants")]
    public List<GameObject> variants;  // e.g. [Happy, Sad, Angry, Surprised]

    /// <summary>
    /// Get a variant sprite by index.
    /// </summary>
    public List<GameObject> GetVariantList()
    {
        return variants;
    }
    public GameObject GetVariantByIndex(int index)
    {
        if (variants == null || variants.Count == 0)
            return defaultPrefab;

        if (index < 0 || index >= variants.Count)
            return defaultPrefab;

        return variants[index];
    }

    /// <summary>
    /// Get a random variant.
    /// </summary>
    public GameObject GetRandomVariant()
    {
        if (variants == null || variants.Count == 0)
            return defaultPrefab;

        int randomIndex = Random.Range(0, variants.Count);
        return variants[randomIndex];
    }

    /// <summary>
    /// Get a variant sprite by its Unity sprite name.
    /// </summary>
    /// <param name="variantName">The name of the sprite variant.</param>
    /// <returns>The matching Sprite if found, otherwise the defaultSprite.</returns>
    public GameObject GetVariantById(string variantName)
    {
        if (variants == null || variants.Count == 0)
            return defaultPrefab;

        foreach (var gb in variants)
        {
            if (gb != null && gb.name.Equals(variantName, System.StringComparison.OrdinalIgnoreCase))
                return gb;
        }

        return defaultPrefab;
    }

    /// <summary>
    /// Get the index of a variant by its name.
    /// Returns -1 if not found.
    /// </summary>
    public int GetVariantIndexByName(string variantName)
    {
        if (variants == null || variants.Count == 0)
            return -1;

        for (int i = 0; i < variants.Count; i++)
        {
            var prefab = variants[i];
            if (prefab != null && prefab.name.Equals(variantName, System.StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1; // Not found
    }
}
