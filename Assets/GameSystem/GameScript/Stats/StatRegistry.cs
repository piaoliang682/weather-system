using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stat Registry", fileName = "StatRegistry")]
public class StatRegistry : ScriptableObject
{
    public List<StatDefinition> registry = new List<StatDefinition>();

    private Dictionary<StatType, StatDefinition> lookup;

    public Dictionary<StatType, StatDefinition> GetLookup()
    {
        if (lookup == null)
        {
            lookup = new Dictionary<StatType, StatDefinition>();
            foreach (var def in registry)
            {
                if (def == null) continue;
                lookup[def.statType] = def;
            }
        }
        return lookup;
    }
}
