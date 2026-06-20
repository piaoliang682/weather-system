using UnityEngine;

public class InventoryDetector : MonoBehaviour
{
    [Header("Target GameObject")]
    public GameObject targetGameObject;

    [Header("Trigger settings")]
    public int targetCount = 3; // You can set this in Inspector

    private PlayerInventory inventoryManager;

    private void Start()
    {
        inventoryManager = PlayerInventory.Instance;

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager.Instance not found!");
        }

        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false); // Start off
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        // Only allow Player to trigger this
        if (!other.CompareTag("Player"))
            return;

        if (inventoryManager == null) return;

        int totalCount = GetTotalItemCount();

        if (totalCount >= targetCount)
        {
            if (targetGameObject != null && !targetGameObject.activeSelf)
            {
                targetGameObject.SetActive(true);
                Debug.Log($"[InventoryTotalTrigger] Activated {targetGameObject.name} because total inventory count == {targetCount}");
            }
        }
        else
        {
            if (targetGameObject != null && targetGameObject.activeSelf)
            {
                targetGameObject.SetActive(false);
                Debug.Log($"[InventoryTotalTrigger] Deactivated {targetGameObject.name} because total inventory count != {targetCount}");
            }
        }
    }

    private int GetTotalItemCount()
    {
        int count = 0;

        foreach (ItemStack stack in inventoryManager.Inventory)
        {
            count += stack.quantity;
        }

        return count;
    }
}
