using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public StackHolder stackHolder;
    private ItemStack itemStack;

    public void Initiate(ItemStack stack)
    {
        stackHolder = GetComponentInChildren<StackHolder>();
        stackHolder.Initiate(stack);
        itemStack = stack;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Get the stack from the object being dragged
        var otherHolder = eventData.pointerDrag?.GetComponent<StackHolder>();
        if (otherHolder == null) return;

        ItemStack sourceStack = otherHolder.GetItemStack();
        ItemStack draggedStack = DragStack.stack;
        ItemStack targetStack = itemStack;

        if (sourceStack == null || draggedStack == null || targetStack == null) return;

        Debug.Log($"[OnDrop] Source Not Empty: {!sourceStack.IsEmpty()}");
        Debug.Log($"[OnDrop] Ghost Not Empty: {!draggedStack.IsEmpty()}");

        // Prevent dropping onto itself
        if (sourceStack == targetStack) return;

        // Case: Valid move within inventory
        if (!sourceStack.IsEmpty() && !draggedStack.IsEmpty())
        {
            Debug.Log("Performing stack change");

            PlayerInventory.Instance.ChangeStacks(sourceStack, draggedStack, targetStack);

            otherHolder.UpdateUI();     // Update source slot
            stackHolder.UpdateUI();     // Update target slot
            DragStack.stack.EmptyStack();
        }
    }
}
