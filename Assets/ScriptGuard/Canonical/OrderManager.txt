using UnityEngine;
using DG.Tweening;

public class OrderManager : MonoBehaviour
{
    public OrderSlot[] slots;

    [Header("Swap Animation")]
    public float swapDuration = 0.5f;
    public Ease swapEase = Ease.InOutQuad;

    private OrderObject firstSelected;
    private OrderObject secondSelected;

    public bool isStart = false;

    void Update()
    {
        if (!isStart) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                OrderObject item = hit.collider.GetComponent<OrderObject>();

                if (item != null)
                {
                    HandleSelection(item);
                }
            }
        }
    }

    public void CallStart()
    {
        isStart = true;
    }

    void HandleSelection(OrderObject item)
    {
        if (firstSelected == null)
        {
            firstSelected = item;
            Debug.Log("First Selected: " + item.correctIndex);
            return;
        }

        secondSelected = item;
        Debug.Log("Second Selected: " + item.correctIndex);
        Debug.Log($"Swapping {firstSelected.correctIndex} <-> {secondSelected.correctIndex}");

        SwapItems(firstSelected, secondSelected);

        firstSelected = null;
        secondSelected = null;

        CheckWin();
    }

    void SwapItems(OrderObject a, OrderObject b)
    {
        // Get the OrderSlot components from the slot transforms
        OrderSlot slotA = a.currentSlot.GetComponent<OrderSlot>();
        OrderSlot slotB = b.currentSlot.GetComponent<OrderSlot>();

        if (slotA == null || slotB == null)
        {
            Debug.LogError("SwapItems: One or both slots are missing OrderSlot component");
            return;
        }

        // Store target positions and rotations
        Vector3 targetPosA = b.transform.position;
        Vector3 targetPosB = a.transform.position;
        Quaternion targetRotA = b.transform.rotation;
        Quaternion targetRotB = a.transform.rotation;

        // Swap slot references
        slotA.currentItem = b;
        slotB.currentItem = a;

        // Swap slot references in OrderObject
        Transform tempSlot = a.currentSlot;
        a.currentSlot = b.currentSlot;
        b.currentSlot = tempSlot;

        // Animate positions and rotations
        Sequence swapSequence = DOTween.Sequence();

        swapSequence.Join(a.transform.DOMove(targetPosA, swapDuration).SetEase(swapEase));
        swapSequence.Join(a.transform.DORotateQuaternion(targetRotA, swapDuration).SetEase(swapEase));
        swapSequence.Join(b.transform.DOMove(targetPosB, swapDuration).SetEase(swapEase));
        swapSequence.Join(b.transform.DORotateQuaternion(targetRotB, swapDuration).SetEase(swapEase));

        swapSequence.OnComplete(() => CheckWin());
    }

    void CheckWin()
    {
        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning("CheckWin: No slots assigned");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentItem == null)
            {
                Debug.Log("Not solved yet: Slot " + i + " is empty");
                return;
            }

            if (slots[i].currentItem.correctIndex != slots[i].slotIndex)
            {
                Debug.Log("Not solved yet: Slot " + i + " has wrong item (expected " + slots[i].slotIndex + ", got " + slots[i].currentItem.correctIndex + ")");
                return;
            }
        }

        Debug.Log("PUZZLE ORDER COMPLETE!");
    }
}