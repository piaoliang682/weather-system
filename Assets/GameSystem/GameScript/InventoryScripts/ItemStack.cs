using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public int id;
    public InventoryItemSO item;
    public int quantity;


    public ItemStack()
    {
        this.id = 0;
        this.item = null;
        this.quantity = 0;
    }
    public ItemStack(int id)
    {
        this.id = id;
        this.item = null;
        this.quantity = 0; // default quantity
    }
    public ItemStack(int id, InventoryItemSO item, int quantity)
    {
        this.id = id;
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsEmpty()
    {
        if (item == null || quantity == 0)
        {
            this.item = null;
            this.quantity = 0;
            return true;
        }
        return false;
    }

    public void EmptyStack()
    {
        this.item = null;
        this.quantity = 0;
    }

    public void RemoveItem(int quantity)
    {

        this.quantity -= quantity;

    }
}