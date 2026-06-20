using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Game/ShopItem", order = 1)]
public class ShopItemSO : ScriptableObject
{
    [Header("Basic Info")]
    public string id;
    [TextArea]
    public string description;
    public Sprite icon;

    [Header("Pricing")]
    public int price;

    [Header("Prefab/Effect")]
    public GameObject itemPrefab;

    [Header("Shop Specific")]
    public bool isLimitedStock = false;
    public int maxStock = 0;

    // Potentially more fields: category, rarity, tags etc.

    // Example method
    public void OnPurchased()
    {

    }



}
