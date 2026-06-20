using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Tooltip("List of items that can be dropped")]
    public List<InventoryItemSO> itemsToDrop;

    [Tooltip("Quantity of each drop")]
    public int quantity = 1;

    // Drop a random item from the list at the GameObject's position
    public void Drop()
    {
        Drop(transform.position);
    }

    // Drop a random item from the list at a specified position
    public void Drop(Vector3 position)
    {
        if (itemsToDrop == null || itemsToDrop.Count == 0)
        {
            Debug.LogWarning("No items assigned to drop!");
            return;
        }

        // Pick a random item from the list
        InventoryItemSO randomItem = itemsToDrop[Random.Range(0, itemsToDrop.Count)];

        if (randomItem == null)
        {
            Debug.LogWarning("Selected item is null!");
            return;
        }

        // Assuming SpawnAsNew is your pooling/spawning method inside the Item class
        randomItem.SpawnItem(position, 1);

        Debug.Log($"Dropped item: {randomItem.name}");
    }
}
