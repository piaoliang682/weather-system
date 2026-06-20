using UnityEngine;
using UnityEngine.Events;
public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string tag = "Player";
    public UnityEvent onTriggerEnter;
    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Ensure it's closed at the start
        }
    }   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
        {
            if (targetObject != null)
                targetObject.SetActive(true); // Open
            onTriggerEnter?.Invoke();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObject.SetActive(false); // Close
        }
    }
}