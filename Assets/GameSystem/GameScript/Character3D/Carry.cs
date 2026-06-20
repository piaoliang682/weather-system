using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Carry : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Radius to search for carryable objects")]
    public float pickRadius = 2f;
    [SerializeField]
    private LayerMask carryLayerMask = ~0;

    [Header("Carry")]
    [Tooltip("Transform under which the carried object will be parented")]
    public Transform carryAnchor;
    [Tooltip("If true, require the player to be in front of the object (relative to the object's forward) to pick it up")]
    public bool requireInFront = false;
    [Tooltip("Dot threshold to determine 'in front' when requireInFront is enabled (1 = exactly in front)")]
    [Range(-1f, 1f)]
    public float frontDotThreshold = 0.0f;

    // runtime state
    private Rigidbody rb;
    private GameObject currentCarried;
    private Rigidbody carriedRigidbody;
    private Collider carriedCollider;
    private bool isCarrying;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (carryAnchor == null)
        {
            Debug.LogWarning("Carry: carryAnchor not assigned in inspector. Pickup will parent to the player transform.");
        }
    }

    // Called by PlayerInputHandler when the interact key is pressed
    public void HandleCarry()
    {
        if (isCarrying)
        {
            Drop();
            return;
        }

        // find nearest carryable object
        Collider[] hits = Physics.OverlapSphere(transform.position, pickRadius, carryLayerMask);
        GameObject nearest = null;
        float bestDist = float.MaxValue;

        foreach (var c in hits)
        {
            // Check if the object has a Carryable component
            if (c.GetComponent<Carryable>() == null)
                continue;

            float d = Vector3.Distance(transform.position, c.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                nearest = c.gameObject;
            }
        }

        if (nearest == null)
        {
            Debug.Log("No carryable object nearby.");
            return;
        }

        if (requireInFront)
        {
            Vector3 toPlayer = (transform.position - nearest.transform.position).normalized;
            float dot = Vector3.Dot(nearest.transform.forward, toPlayer);
            if (dot < frontDotThreshold)
            {
                Debug.Log("Player is not in front of the object.");
                return;
            }
        }

        Pickup(nearest);
    }

    private void Pickup(GameObject go)
    {
        if (go == null) return;

        currentCarried = go;
        carriedRigidbody = currentCarried.GetComponent<Rigidbody>();
        carriedCollider = currentCarried.GetComponent<Collider>();

        if (carriedRigidbody != null)
        {
            carriedRigidbody.velocity = Vector3.zero;
            carriedRigidbody.angularVelocity = Vector3.zero;
            carriedRigidbody.isKinematic = true;
        }

        if (carriedCollider != null)
        {
            carriedCollider.isTrigger = true;
        }

        if (carryAnchor != null)
        {
            currentCarried.transform.SetParent(carryAnchor, worldPositionStays: false);
            currentCarried.transform.localPosition = Vector3.zero;
            currentCarried.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // fallback: parent to player so object follows the player
            currentCarried.transform.SetParent(transform, worldPositionStays: false);
            currentCarried.transform.localPosition = Vector3.zero;
            currentCarried.transform.localRotation = Quaternion.identity;
        }

        isCarrying = true;

        // Invoke the onCarried event
        Carryable carryable = currentCarried.GetComponent<Carryable>();
        if (carryable != null)
        {
            carryable.OnCarried();
        }
    }

    private void Drop()
    {
        if ( currentCarried == null)
        {
            
            isCarrying = false;
            Debug.Log("current caried missing, is carrying");
            return;
        }
        // Invoke the onDropped event
        Carryable carryable = currentCarried.GetComponent<Carryable>();
        if (carryable != null)
        {
            carryable.OnDropped();
        }

        currentCarried.transform.SetParent(null, worldPositionStays: true);

        if (carriedCollider != null)
        {
            carriedCollider.isTrigger = false;
        }

        if (carriedRigidbody != null)
        {
            carriedRigidbody.isKinematic = false;
            carriedRigidbody.AddForce(transform.forward * 1.5f + Vector3.up * 0.5f, ForceMode.VelocityChange);
        }

        currentCarried = null;
        carriedRigidbody = null;
        carriedCollider = null;
        isCarrying = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickRadius);
        if (carryAnchor != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(carryAnchor.position, 0.05f);
        }
    }
}
