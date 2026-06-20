using UnityEngine;

public class ChestInventory : InventoryBase
{
    [SerializeField] private InventoryPreset inventoryPreset;
    [SerializeField] private bool useSaveData = false;
    [SerializeField] private string chestId = "Chest_01";

    private void Awake()
    {
        inventoryId = chestId;
        isUsingPref = useSaveData;
        slots = GetComponentsInChildren<Slot>();

        if (useSaveData)
        {
            LoadInventoryFromPrefs();
        }
        else if (inventoryPreset != null)
        {
            InitializeInventory(inventoryPreset.itemList);
        }
        else
        {
            InitializeInventory(); // Empty chest
        }
    }

    private void OnDestroy()
    {
        if (isUsingPref)
        {
            SaveInventoryToPrefs();
        }
    }

    public void OpenChest()
    {
        Open();
    }


    public void ResetChest()
    {
        ClearInventory();
    }
}
