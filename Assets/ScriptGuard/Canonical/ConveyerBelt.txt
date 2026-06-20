using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConveyorBelt : MonoBehaviour
{
    [Header("Conveyor Settings")]
    public float speed = 2f;

    private readonly HashSet<ConveyorItem> items = new();

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        ConveyorItem item = other.GetComponent<ConveyorItem>();

        if (item != null)
        {
            Debug.Log($"Item {item.name} entered conveyor belt.");
            items.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ConveyorItem item = other.GetComponent<ConveyorItem>();

        if (item != null)
        {
            items.Remove(item);
        }
    }

    private void Update()
    {
        Vector3 moveDirection = transform.forward;

        foreach (var item in items)
        {
            if (item == null)
                continue;

            item.transform.position +=
                moveDirection * speed * Time.deltaTime;
        }
    }
}