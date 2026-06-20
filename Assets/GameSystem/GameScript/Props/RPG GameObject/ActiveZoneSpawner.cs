using UnityEngine;
using System.Collections;

/// <summary>
/// Continuously spawns prefabs at intervals within defined zone colliders.
/// Inherits shared logic from ZoneSpawnerBase.
/// </summary>
public class ActiveZoneSpawner : ZoneSpawnerBase
{
    [Header("Spawn Settings")]
    [Tooltip("Seconds between each spawn.")]
    public float spawnInterval = 2f;

    [Tooltip("Total number of objects to spawn. Set to 0 for infinite spawning.")]
    public int maxSpawnCount = 0;

    [Tooltip("Automatically start spawning when enabled.")]
    public bool autoStartOnEnable = true;

    private int spawnedCount = 0;
    private bool spawning = false;
    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        if (autoStartOnEnable)
            StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    /// <summary>
    /// Begins the spawn loop coroutine.
    /// </summary>
    public void StartSpawning()
    {
        if (spawning)
            return;

        if (prefabsToSpawn.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning($"{name}: Missing prefabs or spawn zones!");
            return;
        }

        spawning = true;
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// Stops the spawn loop coroutine.
    /// </summary>
    public void StopSpawning()
    {
        if (!spawning)
            return;

        spawning = false;
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    /// <summary>
    /// Coroutine responsible for repeatedly spawning prefabs.
    /// </summary>
    private IEnumerator SpawnLoop()
    {
        Transform parent = spawnParent != null ? spawnParent : transform;
        spawnedCount = 0;

        while (spawning && (maxSpawnCount == 0 || spawnedCount < maxSpawnCount))
        {
            GameObject prefab = GetRandomPrefab();
            Collider zone = GetRandomZone();
            if (prefab == null || zone == null)
                yield break;

            Vector3 spawnPos = GetRandomPointInCollider(zone);
            Instantiate(prefab, spawnPos, parent.transform.rotation, parent);

            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }

        spawning = false;
    }

    /// <summary>
    /// Triggers one spawn cycle manually.
    /// </summary>
    public override void TriggerSpawn()
    {
        StartSpawning();
    }
}
