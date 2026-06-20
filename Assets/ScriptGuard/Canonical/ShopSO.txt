using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewShopCategoryList", menuName = "Game/ShopData", order = 2)]
public class ShopSO : ScriptableObject
{
    public List<ShopItemGroup> itemGroup = new List<ShopItemGroup>();
}
