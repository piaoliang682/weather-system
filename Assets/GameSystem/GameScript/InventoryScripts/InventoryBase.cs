using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    protected List<ItemStack> inventoryStacks = new List<ItemStack>();
    public List<ItemStack> Inventory => inventoryStacks;
    public int maxStack;
    public bool isUsingPref = false;
    public string inventoryId = "Default";

    [SerializeField] protected Slot[] slots;

    public virtual void InitializeInventory(ItemStack[] preset = null)
    {
        inventoryStacks = new List<ItemStack>();
        maxStack = slots.Length;
        for (int i = 0; i < maxStack; i++)
        {
            ItemStack newStack = new ItemStack { id = i };

            // Apply preset items if available
            if (preset != null && i < preset.Length)
            {
                newStack.item = preset[i].item;
                newStack.quantity = preset[i].quantity;
            }

            inventoryStacks.Add(newStack);

            // Link stack to UI slot
            slots[i].Initiate(newStack);
            slots[i].stackHolder.SetHostInventory(this);
        }
    }

    public virtual void ChangeStacks(ItemStack a, ItemStack ghostStack, ItemStack b)
    {
        a.quantity -= ghostStack.quantity;

        if (b.IsEmpty())
        {
            b.item = ghostStack.item;
            b.quantity = ghostStack.quantity;
        }
        else if (b.item.stackable && a.item == b.item)
        {
            b.quantity += ghostStack.quantity;
        }
        else
        {
            a.item = b.item;
            a.quantity = b.quantity;

            b.item = ghostStack.item;
            b.quantity = ghostStack.quantity;
        }

        if (isUsingPref)
            SaveInventoryToPrefs();
    }

    public virtual void Add(InventoryItemSO item, int quantity)
    {
        if (item.stackable)
        {
            foreach (var stack in inventoryStacks)
            {
                if (stack.item == item)
                {
                    stack.quantity += quantity;
                    slots[stack.id].stackHolder.UpdateUI();

                    if (isUsingPref)
                        SaveInventoryToPrefs();
                    return;
                }
            }
        }

        foreach (var stack in inventoryStacks)
        {
            if (stack.IsEmpty())
            {
                stack.item = item;
                stack.quantity = quantity;
                slots[stack.id].stackHolder.UpdateUI();

                if (isUsingPref)
                    SaveInventoryToPrefs();
                return;
            }
        }
    }

    public virtual ItemStack Remove(string itemId, int quantity)
    {
        foreach (var stack in Inventory)
        {
            if (stack != null && stack.id.ToString() == itemId)
            {
                Debug.LogWarning($"removing {itemId}.");
                if (stack.quantity >= quantity)
                {
                    ItemStack removedStack = new ItemStack(stack.id, stack.item, quantity);
                    stack.RemoveItem(quantity);
                    if (stack.quantity == 0) 
                        stack.item = null;
                    slots[stack.id].stackHolder.UpdateUI();
                    Debug.Log(stack.id + "updated");
                    if (isUsingPref)
                        SaveInventoryToPrefs();
                    return removedStack;
                }




            }
        }

        Debug.LogWarning($"Item {itemId} not found or not enough quantity.");
        return null;
    }

    public virtual InventoryItemSO UseItem(string itemId)
    {

        ItemStack itemStack = Remove(itemId, 1);
        Debug.LogWarning($"Item {itemId} used, current {itemStack.quantity}");
        InventoryItemSO inventoryItemSO = itemStack.item;

        if (inventoryItemSO == null)
            return null;

        // If the item has a buff, apply it
        //if (inventoryItemSO.buff != null)
        //{
        //    BuffController buffController = GetComponentInParent<BuffController>();
        //    buffController.ApplyBuff(inventoryItemSO.buff);
        //}

        return inventoryItemSO;
    }
    public virtual void ClearInventory()
    {
        for (int i = 0; i < inventoryStacks.Count; i++)
        {
            inventoryStacks[i].EmptyStack();

            if (slots != null && i < slots.Length)
            {
                slots[i].stackHolder.UpdateUI();
            }
        }

        if (isUsingPref)
            SaveInventoryToPrefs();
    }
    public virtual void RemoveStack(ItemStack stack)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i] == stack)
            {
                stack.EmptyStack();
                slots[i].stackHolder.UpdateUI();

                if (isUsingPref)
                    SaveInventoryToPrefs();
                return;
            }
        }
    }

    public virtual void Toggle()
    {
            gameObject.SetActive(!gameObject.activeSelf);
    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    public virtual void SaveInventoryToPrefs()
    {
        if (!isUsingPref) return;

        InventorySaveData saveData = new InventorySaveData();
        foreach (var stack in inventoryStacks)
        {
            saveData.slots.Add(new InventorySlotData
            {
                itemId = stack.item != null ? stack.item.id : "",
                quantity = stack.quantity
            });
        }

        PlayerPrefs.SetString($"{inventoryId}_inventory", JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }

    public virtual void LoadInventoryFromPrefs()
    {
        if (!isUsingPref)
        {
            InitializeInventory();
            return;
        }

        string json = PlayerPrefs.GetString($"{inventoryId}_inventory", "");
        var saveData = JsonUtility.FromJson<InventorySaveData>(json);

        for (int i = 0; i < inventoryStacks.Count; i++)
        {
            inventoryStacks[i] = new ItemStack { id = i };

            if (saveData != null && i < saveData.slots.Count)
            {
                var saved = saveData.slots[i];
                if (!string.IsNullOrEmpty(saved.itemId) && saved.quantity > 0)
                {
                    inventoryStacks[i].item = ItemDatabase.GetItemById(saved.itemId);
                    inventoryStacks[i].quantity = saved.quantity;
                }
            }

            if (slots != null && i < slots.Length)
                slots[i].Initiate(inventoryStacks[i]);
        }
    }

    public virtual void ClearInventoryPrefs()
    {
        if (!isUsingPref) return;

        PlayerPrefs.DeleteKey($"{inventoryId}_inventory");
        PlayerPrefs.Save();
    }
}
