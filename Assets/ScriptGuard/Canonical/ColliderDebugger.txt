using UnityEngine;

public class ColliderDebugger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {

            Debug.Log($"Item {other.name} entered.");

    }
}