using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class EliminationGameMode : MonoBehaviour
{
    public static EliminationGameMode Instance;

    [Header("Elimination Mode Settings")]
    public string enemyTag = "Enemy";          // Tag to identify enemies
    private HashSet<GameObject> enemies = new HashSet<GameObject>();

    private int totalPossibleScore;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


    }

    private void Start()
    {
        Instantiate(GameReference.Game);
        LevelManager.Instance.SpawnLevel();
       StartGame();
        GameOverUIManager.Instance.Initiate();
        enemies = GameReference.EnemyRegistry;

    }

    public void StartGame()
    {        
        foreach (var stats in enemies)
        {
            totalPossibleScore += 10;
        }
        GameReference.StarThreholds = new List<int>
        {
             Mathf.RoundToInt(totalPossibleScore * 0.5f),
            Mathf.RoundToInt(totalPossibleScore * 0.75f),
            Mathf.RoundToInt(totalPossibleScore)
        };

    }
    /// <summary>
    /// Checks if all enemies are destroyed / eliminated.
    /// </summary>
    public void CheckWinCondition()
    {
        if (enemies == null)
        {
            Debug.LogWarning("Enemy container not assigned!");
            return;
        }

        int enemyCount = 0;
        foreach (var  child in enemies)
        {
            if (child.gameObject.CompareTag(enemyTag))
                enemyCount++;
        }
        Debug.Log("enemy count"+ enemyCount);
        if (enemyCount == 0)
        {
            SystemGameManager.Instance.GameWin();
        }
        if (GameReference.Player.GetComponent<CharacterStats>().GetCurrentHealth()<=0)
        {
            SystemGameManager.Instance.GameLose();
        }
    }

}
