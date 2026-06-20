[System.Serializable]
public class ShopItemInstance
{
    public string itemId;             // optional unique id (e.g. itemData.name)
    public ShopItemSO itemData;       // reference to the original data
    public int currentStock;
    public bool isClaimed;            // for daily rewards or ownership

    public ShopItemInstance(ShopItemSO data)
    {
        itemData = data;
        itemId = data.id;
        currentStock = data.isLimitedStock ? data.maxStock : int.MaxValue;
        isClaimed = false;
    }

    public bool CanClaim(int quantity = 1)
    {
        return currentStock >= quantity;
    }
    public bool IsClaimed()
    {
        return isClaimed;
    }
    public void Claim(int quantity = 1)
    {
        if (CanClaim(quantity))
        {
            isClaimed = true;
            currentStock -= quantity;
            itemData.OnPurchased();
        }
    }

    public void SetIsClaimed(bool b)
    {
        isClaimed = b;
    }
    // Helper: Convert to JSON-serializable data
    public SerializableShopItemInstance ToSerializable()
    {
        return new SerializableShopItemInstance()
        {
            id = itemData != null ? itemData.id : "",
            claimed = isClaimed,
            currentStock = currentStock
        };
    }

}

[System.Serializable]
public class SerializableShopItemInstance
{
    public string id;
    public bool claimed;
    public int currentStock;
}
