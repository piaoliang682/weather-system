using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class StackHolder : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    private ItemStack itemStack;
    public TMP_Text textComponent;
    public  Image itemImage;
    private Vector2 mouseOffset;
    private InventoryBase hostInventory;
    // Track hover state
    private static StackHolder hoveredHolder;

    public void Start()
    {
        //textComponent = GetComponentInChildren<TMP_Text>();
        //itemImage = GetComponent<Image>();
    }
    public void Initiate(ItemStack stack)
    {

        //textComponent = GetComponentInChildren<TMP_Text>();
        //itemImage = GetComponent<Image>();
        this.itemStack = stack;

        UpdateUI();
        DragStack.gameObject.SetActive(false);
    }

    public bool UpdateUI()
    {
        if (!itemStack.IsEmpty())
        {
            EnableUI();
            return true;
        }
        else
        {
            DisableUI();
            return false;
        }
    }

    public void EnableUI()
    {
        gameObject.name = itemStack.item.name;
        itemImage.sprite = itemStack.item.itemImage;
        itemImage.color = new Color(255, 255, 255, 255);
        textComponent.text = (itemStack.quantity > 1) ? itemStack.quantity + "" : "";
    }

    public void DisableUI()
    {
        gameObject.name = "Empty Stack";
        itemImage.color = new Color(255, 255, 255, 0);
        textComponent.text = "";
    }
    public ItemStack GetItemStack()
    {
        return itemStack;
    }

    public void SetHostInventory(InventoryBase inventory)
    {
        hostInventory=inventory;
    }
    public InventoryBase GetHostInventory()
    {
        return hostInventory;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"stack {itemStack.item != null}");
        mouseOffset = eventData.position - (Vector2)transform.position;

        DragStack.stack.id = itemStack.id;
        DragStack.stack.item = itemStack.item;

        if (Input.GetMouseButton(0))
            DragStack.stack.quantity = itemStack.quantity;
        else if (Input.GetMouseButton(2))
            DragStack.stack.quantity = (itemStack.quantity / 2);
        else if (Input.GetMouseButton(1))
            DragStack.stack.quantity = 1;
        else
            DragStack.stack.quantity = 1;

        if ((itemStack.quantity - DragStack.stack.quantity) <= 0)
        {
            DisableUI();
        }
        else
        {
            textComponent.text = ((itemStack.quantity - DragStack.stack.quantity) > 1) ? (itemStack.quantity - DragStack.stack.quantity) + "" : "";
        }

        DragStack.UpdateUI();
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragStack.Move(eventData.position - mouseOffset);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(itemStack.quantity);
        if (eventData.pointerCurrentRaycast.gameObject == null && itemStack.item.IsDropable())
        {
            Debug.Log("drop");
            Vector3 dropPosition = GetWorldDropPosition();
            itemStack.item.SpawnFromInventory(dropPosition);
            itemStack.quantity -= DragStack.stack.quantity;



        }
            DragStack.gameObject.SetActive(false);
        UpdateUI();

    }
    private Vector3 GetWorldDropPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point; // Hit point on ground or terrain
        }

        // Fallback: 2 units in front of camera
        return Camera.main.transform.position + Camera.main.transform.forward * 2f;
    }
    // --- New hover detection ---
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredHolder = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoveredHolder == this)
            hoveredHolder = null;
    }

    public static StackHolder GetHoveredHolder()
    {
        return hoveredHolder;
    }
}