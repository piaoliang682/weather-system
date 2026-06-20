using UnityEngine;

/// <summary>
/// Spawns a set number of prefabs one time within defined zone colliders.
/// Inherits from ZoneSpawnerBase.
/// </summary>
public class OneOffZoneSpawner : ZoneSpawnerBase
{
    [Header("Spawn Settings")]
    [Tooltip("Number of objects to spawn when triggered.")]
    public int totalSpawnCount = 5;

    [Tooltip("Automatically spawn when the spawner becomes active.")]
    public bool autoSpawnOnEnable = true;

    private bool hasSpawned = false;

    private void OnEnable()
    {
        if (autoSpawnOnEnable)
            TriggerSpawn();
    }

    /// <summary>
    /// Performs the one-time spawn action.
    /// </summary>
    public override void TriggerSpawn()
    {
        if (hasSpawned)
            return;

        if (prefabsToSpawn.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning($"{name}: Missing prefabs or spawn zones!");
            return;
        }

        Transform parent = spawnParent != null ? spawnParent : transform;

        for (int i = 0; i < totalSpawnCount; i++)
        {
            GameObject prefab = GetRandomPrefab();
            Collider zone = GetRandomZone();
            if (prefab == null || zone == null)
                continue;

            Vector3 spawnPos = GetRandomPointInCollider(zone);
            Instantiate(prefab, spawnPos, Quaternion.identity, parent);
            Debug.Log("spawn");
        }

        hasSpawned = true;
    }

    /// <summary>
    /// Allows the zone to spawn again (for reuse or debugging).
    /// </summary>
    public void ResetSpawner()
    {
        hasSpawned = false;
    }
}
