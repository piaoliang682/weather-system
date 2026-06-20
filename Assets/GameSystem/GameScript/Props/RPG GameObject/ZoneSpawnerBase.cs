using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for all zone spawners. 
/// Handles shared logic like collider collection, random spawn position, and prefab reference management.
/// </summary>
public abstract class ZoneSpawnerBase : MonoBehaviour
{
    [Header("Spawn Prefabs")]
    [Tooltip("List of prefabs that can be spawned by this zone.")]
    public List<GameObject> prefabsToSpawn = new List<GameObject>();

    [Header("Spawn Parent")]
    [Tooltip("Optional parent to hold spawned objects. If null, defaults to this.transform.")]
    public Transform spawnParent;

    /// <summary>
    /// All colliders under this spawner that define spawnable areas.
    /// </summary>
    protected List<Collider> spawnAreas = new List<Collider>();

    protected virtual void Awake()
    {
        // Collect all child colliders as spawn zones
        spawnAreas.AddRange(GetComponentsInChildren<Collider>());
    }

    /// <summary>
    /// Returns a random prefab from the list.
    /// </summary>
    protected GameObject GetRandomPrefab()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Count == 0)
        {
            Debug.LogWarning($"{name}: No prefabs assigned!");
            return null;
        }

        return prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];
    }

    /// <summary>
    /// Returns a random collider zone from the list.
    /// </summary>
    protected Collider GetRandomZone()
    {
        if (spawnAreas == null || spawnAreas.Count == 0)
        {
            Debug.LogWarning($"{name}: No spawn zones found!");
            return null;
        }

        return spawnAreas[Random.Range(0, spawnAreas.Count)];
    }

    /// <summary>
    /// Returns a random spawn position inside the given collider.
    /// Supports BoxCollider, SphereCollider, CapsuleCollider, or defaults to bounds.
    /// </summary>
    protected Vector3 GetRandomPointInCollider(Collider col)
    {
        if (col is BoxCollider box)
        {
            Vector3 center = box.transform.TransformPoint(box.center);
            Vector3 size = Vector3.Scale(box.size, box.transform.lossyScale);
            return center + new Vector3(
                Random.Range(-size.x / 2, size.x / 2),
                Random.Range(-size.y / 2, size.y / 2),
                Random.Range(-size.z / 2, size.z / 2)
            );
        }
        else if (col is SphereCollider sphere)
        {
            Vector3 center = sphere.transform.TransformPoint(sphere.center);
            return center + Random.insideUnitSphere * sphere.radius * sphere.transform.lossyScale.x;
        }
        else if (col is CapsuleCollider capsule)
        {
            Vector3 center = capsule.transform.TransformPoint(capsule.center);
            Vector3 dir = capsule.direction switch
            {
                0 => Vector3.right,
                1 => Vector3.up,
                2 => Vector3.forward,
                _ => Vector3.up
            };

            float halfHeight = Mathf.Max(0, capsule.height / 2f - capsule.radius);
            Vector3 pointAlong = center + dir * Random.Range(-halfHeight, halfHeight);
            return pointAlong + Random.insideUnitSphere * capsule.radius * capsule.transform.lossyScale.x;
        }
        else
        {
            // Generic fallback
            Bounds b = col.bounds;
            return b.center + new Vector3(
                Random.Range(-b.extents.x, b.extents.x),
                Random.Range(-b.extents.y, b.extents.y),
                Random.Range(-b.extents.z, b.extents.z)
            );
        }
    }

    /// <summary>
    /// Abstract method for subclasses to implement specific spawn behavior.
    /// </summary>
    public abstract void TriggerSpawn();
}
