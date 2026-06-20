using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DragStack
{
    public static GameObject gameObject;
    public static Transform transform;
    public static ItemStack stack = new ItemStack();
    private static Image imageComponent;
    private static Text textComponent;

    public static void Initialize(GameObject gameObject)
    {
        DragStack.gameObject = gameObject;
        DragStack.transform = DragStack.gameObject.transform;
        DragStack.imageComponent = DragStack.gameObject.GetComponentInChildren<Image>();
        textComponent = DragStack.gameObject.GetComponentInChildren<Text>();
    }
    public static void UpdateUI()
    {
        if(!stack.IsEmpty())
        {
            gameObject.name = stack.item.name;
            imageComponent.sprite = stack.item.itemImage;
            imageComponent.color = new Color(255, 255, 255, 255);
            textComponent.text = (stack.quantity > 1) ? stack.quantity + "" : "";
            DragStack.gameObject.SetActive(true);
        }
    }
    public static void Move(Vector2 position)
    {
        DragStack.transform.position = position;
    }
}