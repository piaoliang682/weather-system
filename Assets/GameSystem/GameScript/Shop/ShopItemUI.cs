using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;        // if you're using TextMeshPro; otherwise use UnityEngine.UI.Text

public class ShopItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject focusGB;
    [SerializeField] private GameObject clearGB;
    [SerializeField] private GameObject specialGB;
    private ShopItemSO itemData;
    private System.Action<ShopItemSO> onBuyCallback;

    /// <summary>
    /// Initialize this slot with item data and callback for buying.
    /// </summary>
    public void Initialize(ShopItemSO item, System.Action<ShopItemSO> onBuy)
    {
        itemData = item;
        onBuyCallback = onBuy;

        // Set UI values
        if (iconImage != null)
            iconImage.sprite = item.icon;

        if (nameText != null)
            nameText.text = item.id;

        if (priceText != null)
            priceText.text = item.price.ToString();

        // Set up button
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(HandleBuyClicked);

        // Option: disable the button if item is out of stock or cannot be bought
        if (item.isLimitedStock && item.maxStock <= 0)
        {
            buyButton.interactable = false;
            // optionally show ¡°Sold Out¡± label
        }
        else
        {
            buyButton.interactable = true;
        }
    }


    public void SetFocus(bool b)
    {
        focusGB.SetActive(b);
    }
    public void SetSpecial(bool b)
    {
        specialGB.SetActive(b);
    }
    public void SetClear(bool b)
    {
        focusGB.SetActive(false);
        clearGB.SetActive(b);

    }
    public void SetClaimButton(UnityAction onClick)
    {
        buyButton.onClick.RemoveAllListeners();
        if (onClick != null)
            buyButton.onClick.AddListener(onClick);
    }

    private void HandleBuyClicked()
    {
        if (onBuyCallback != null && itemData != null)
        {
            onBuyCallback.Invoke(itemData);
        }
    }
}
