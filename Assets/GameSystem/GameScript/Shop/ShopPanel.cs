using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanel : InteractivePanelBase
{
    [Header("References")]
    [SerializeField] private ShopSO shopData;   // your ScriptableObject that groups categories & items
    [SerializeField] private Transform shopContainer;          // parent transform for instantiated UI slots


    private List<GameObject> instantiatedSlots = new List<GameObject>();

    void Start()
    {
        GenerateUI();
        UpdateCurrencyUI();
    }

    private void GenerateUI()
    {
        // Clear existing slots
        foreach (var slot in instantiatedSlots)
        {
            Destroy(slot);
        }
        instantiatedSlots.Clear();

        // For each category in the shopData.itemGroup
        foreach (var group in shopData.itemGroup)
        {
            // Instantiate header for the group
            GameObject groupGO = Instantiate(group.GroupPrefab, shopContainer.transform);

            // **Find the TMP text component on this header and set it**
            ShopItemGroupUI shopItemGroupUI = groupGO.GetComponentInChildren<ShopItemGroupUI>();
            shopItemGroupUI.groupName.text= group.groupName;  // or group.displayName etc


            foreach (var item in group.items)
            {
                GameObject itemGO = Instantiate(item.itemPrefab, shopItemGroupUI.itemHolder);
                var slotUI = itemGO.GetComponent<ShopItemUI>();
                if (slotUI != null)
                {
                    slotUI.Initialize(item, OnItemPurchased);
                }
                instantiatedSlots.Add(itemGO);
            }
        }
    }

    private void OnItemPurchased(ShopItemSO item)
    {
        //if (playerCurrency >= item.price)
        //{
        //    playerCurrency -= item.price;
        //    item.OnPurchased();
        //    // TODO: Add the item to player's inventory or apply effect
        //    Debug.Log($"Purchased item: {item.itemName}");
        //    UpdateCurrencyUI();
        //}
        //else
        //{
        //    Debug.Log($"Not enough currency to purchase: {item.itemName}");
        //}
    }

    private void UpdateCurrencyUI()
    {
        //if (currencyText != null)
        //{
        //    currencyText.text = $"Gold: {playerCurrency}";
        //}
    }


}
