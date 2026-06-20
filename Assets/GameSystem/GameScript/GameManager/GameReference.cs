using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameReference : MonoBehaviour
{
    public static GameReference Instance { get; private set; }


    [Header("InScene Static Asset References")]


    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject confirmationPopUp;
    [SerializeField] private GameObject messagePopUp;

    [SerializeField] private GameObject game;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameSceneRegistry sceneRegistry;
    [SerializeField] private LevelSO levelSO;
    //[SerializeField] private BuffRegistrySO buffRegistry;
    [SerializeField] private AudioRegistry audioRegistry;
    [Header("Global Pauseable Objects")]
    [SerializeField] private List<GameObject> pauseObjects = new List<GameObject>();

    // Dictionary to track pauseable objects (avoid duplicates)
    public static Dictionary<GameObject, bool> PauseObjects { get; private set; } = new Dictionary<GameObject, bool>();
    public static Dictionary<string, string> PrefabVariantDict { get; private set; } = new Dictionary<string, string>();

    public static HashSet<GameObject> EnemyRegistry { get; private set; } = new HashSet<GameObject>();
    public static int EnemyEliminationCount { get; set; }
    public static GameObject PopUp { get; set; }
    public static GameObject MessagePopUp { get; set; }

    public static GameObject ConfirmationPopUp { get; set; }
    public static GameObject Player { get; set; }
    public static AudioSource UIAudioSource { get; set; }
    public static AudioSource BGMAudioSource { get; set; }

    public static AudioSource AmbienceAudioSource { get; set; }
    //public static List<int> PlayerVariantGroupList { get; set; }
    public static GameObject Game { get; set; }
    public static GameObject LevelPrefab { get; set; }
    public static Transform CameraTransform { get; set; }
    public static LevelSO LevelSO { get; set; }
    public static AudioRegistry AudioRegistry { get; set; }
    //public static BuffRegistrySO BuffRegistry { get; set; }

    public static SpawnPointData CurrentSpawnPoint{ get; set; }
    public static int CurrentLevelIndex { get; set; }

    public static float CurrentScore { get;
        set;
    }
    public static List<int> StarThreholds { get; set; } = new List<int> { 100, 200, 300 };

    public static Dictionary<string, float> TempGameStats { get; set; } = new Dictionary<string, float>();
    public static string StartScene { get; set; }
    public static string LobbyScene { get; set; }
    public static string MainScene { get; set; }
    public static string LevelSelectScene { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        Instance = this;
        PopUp = popUp;
        ConfirmationPopUp = confirmationPopUp;
        MessagePopUp = messagePopUp;
        //BuffRegistry = buffRegistry;
        AudioRegistry = audioRegistry;

        Game = game;
        LevelPrefab = levelPrefab;
        LevelSO = levelSO;
        StartScene = sceneRegistry.startSceneName;
        LobbyScene = sceneRegistry.lobbySceneName;


        //CameraTransform = cameraTransform;

        // Add initial pause objects from inspector
        foreach (var obj in pauseObjects)
            RegisterPauseObject(obj);
    }
    private void Update()
    {

    }
    // Register an object to be paused/resumed
    public static void RegisterPauseObject(GameObject obj)
    {
        if (obj == null) return;

        if (!PauseObjects.ContainsKey(obj))
            PauseObjects.Add(obj, obj.activeSelf); // store original active state
    }

    // Unregister object
    public static void UnregisterPauseObject(GameObject obj)
    {
        if (obj == null) return;

        if (PauseObjects.ContainsKey(obj))
            PauseObjects.Remove(obj);
    }

    // Scene loading helpers
    public static void LoadStartScene() => SceneManager.LoadScene(StartScene);
    public static void LoadGameScene() => SceneManager.LoadScene(MainScene);
    public static void LoadLevelSelect() => SceneManager.LoadScene(LevelSelectScene);

    public void SetLevelPrefab(GameObject gb) => levelPrefab = gb;



}

[System.Serializable]
public class SpawnPointData
{
    public string SceneName;
    public Vector3 Position;
    public Quaternion Rotation;
    // Constructor
    public SpawnPointData(string sceneName, Vector3 position, Quaternion rotation)
    {
        SceneName = sceneName;
        Position = position;
        Rotation = rotation;
    }

}
