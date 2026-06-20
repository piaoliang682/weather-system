using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game/Level Item", fileName = "NewLevelItem")]
public class LevelItemSO : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("Unique ID for this level (e.g. Level_1, Tutorial, BossStage)")]
    public string levelId;

    [Header("Level Info")]
    public string levelName;
    public string sceneName;
    [TextArea] public string requirementText;

    [Header("Visuals")]
    public Sprite previewImage;

    [Header("Gameplay")]
    public GameObject levelPrefab;
    [Header("default")]
    public bool isLocked=true;

}
