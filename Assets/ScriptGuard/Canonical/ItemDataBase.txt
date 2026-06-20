using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [Tooltip("Assign all item assets here manually.")]
    public List<InventoryItemSO> items;  // Drag items in the inspector

    private static Dictionary<string, InventoryItemSO> itemLookup;

    private void Awake()
    {
        if (itemLookup != null) return; // Already initialized

        itemLookup = new Dictionary<string, InventoryItemSO>();

        foreach (var item in items)
        {
            if (item != null && !string.IsNullOrEmpty(item.id))
            {
                if (!itemLookup.ContainsKey(item.id))
                {
                    itemLookup[item.id] = item;
                }
                else
                {
                    Debug.LogWarning($"Duplicate Item ID detected: {item.id}");
                }
            }
        }

        Debug.Log($"[ItemDatabase] Loaded {itemLookup.Count} items manually.");
    }

    public static InventoryItemSO GetItemById(string id)
    {
        if (itemLookup != null && itemLookup.TryGetValue(id, out var item))
        {
            return item;
        }

        Debug.LogWarning($"Item with ID '{id}' not found.");
        return null;
    }
}
