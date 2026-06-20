using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HandleUse()
    {
            StackHolder hovered = StackHolder.GetHoveredHolder();

            if (hovered != null && hovered.GetItemStack() != null)
            {
                Debug.Log($"Using item: {hovered.GetItemStack().item.name}");
                hovered.GetHostInventory().UseItem(hovered.GetItemStack().id.ToString()); 
            }
    }
}
