using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "1", menuName = "Game/PrefabVariantGroupDataSO")]
public class PrefabVariantGroupRegistrySO : ScriptableObject
{
    [Header("Basic Info")]
    public string prefabName;

    [Header("Sprite Variations")]
    public List<PrefabVariantGroupSO> variations = new List<PrefabVariantGroupSO>();

    /// <summary>
    /// Gets a SpriteVariantSO by its spriteId (name).
    /// </summary>
    /// <param name="name">The spriteId of the variation to find.</param>
    /// <returns>The matching SpriteVariantSO, or null if not found.</returns>
    public PrefabVariantGroupSO GetVariationGroupByName(string name)
    {
        // Case-insensitive lookup for flexibility
        return variations.Find(v => v.varientGroupName.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }
}
