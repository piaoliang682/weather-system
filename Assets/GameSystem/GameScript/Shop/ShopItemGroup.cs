// Category list SO
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopCategoryList", menuName = "Game/ShopItemGroup", order = 2)]
public class ShopItemGroup : ScriptableObject
{

    public string groupName = "None";
    public GameObject GroupPrefab;
    public List<ShopItemSO> items = new List<ShopItemSO>();



}
