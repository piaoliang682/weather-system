using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance { get; private set; }

    //[Header("Assign Spawn Points in Inspector")]
    //[SerializeField]
    //private List<SpawnPointData> spawnPoints = new List<SpawnPointData>();

    //[Header("Current Active Spawn Point")]
    //[SerializeField]
    //private SpawnPointData currentSpawnPoint;

    //public SpawnPointData CurrentSpawnPoint => currentSpawnPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Automatically sets the current spawn point for the active scene
    /// </summary>
    public void SetCurrentSpawnPoint(Transform location)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SetCurrentSpawnPoint(sceneName,location);
    }

    /// <summary>
    /// Set by scene name
    /// </summary>
    public void SetCurrentSpawnPoint(string sceneName,Transform location)
    {

        GameReference.CurrentSpawnPoint = new SpawnPointData(sceneName,location.position,location.rotation);


    }


    /// <summary>
    /// Set directly by reference
    /// </summary>
    public void SetCurrentSpawnPoint(SpawnPointData spawn)
    {

        GameReference.CurrentSpawnPoint = spawn;
    }
    public SpawnPointData GetCurrentSpawnPoint()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        return GetCurrentSpawnPoint(sceneName);
    }

    public SpawnPointData GetCurrentSpawnPoint(string sceneName)
    {
        if (GameReference.CurrentSpawnPoint == null) return null;
        if (GameReference.CurrentSpawnPoint.SceneName== sceneName)
        {
            return GameReference.CurrentSpawnPoint;
        }
        return null;
    }
}

