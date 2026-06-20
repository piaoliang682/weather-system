using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionLine : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject itemPrefab;      // A/B/C prefab (can randomize later)
    public Transform spawnPoint;
    public Transform endPoint;

    public float spawnInterval = 1.5f;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private List<Transform> items = new List<Transform>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        MoveItems();
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnItem()
    {
        GameObject obj = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        items.Add(obj.transform);
    }

    void MoveItems()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Transform t = items[i];

            if (t == null)
            {
                items.RemoveAt(i);
                continue;
            }

            // Move toward end point
            t.position = Vector3.MoveTowards(
                t.position,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );

            // Destroy when reached destination
            if (Vector3.Distance(t.position, endPoint.position) < 0.05f)
            {
                Destroy(t.gameObject);
                items.RemoveAt(i);
            }
        }
    }
}