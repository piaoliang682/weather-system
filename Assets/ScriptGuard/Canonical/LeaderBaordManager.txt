using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    public GameObject panel;
    public Button toggleButton;

    public Button closeButton;

    [Header("Leaderboard UI")]
    public Transform entriesParent;     // Parent GameObject containing the entries
    public GameObject entryPrefab;      // Prefab for a single leaderboard entry row

    [Header("Settings")]
    public int leaderboardSize = 5;

    private const string leaderboardKey = "LeaderboardEntry";

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        toggleButton.onClick.AddListener(ToggleLeaderBoard);
        closeButton.onClick.AddListener(HideLeaderBoard);
        ShowLeaderboard();
        HideLeaderBoard();
    }




    // Submits the score with the player's name
    public void SetEntry(string name,int score)
    {
        Debug.Log(name+score);

        List<LeaderboardEntry> leaderboard = LoadLeaderboard();

        // Add new entry
        leaderboard.Add(new LeaderboardEntry { playerName = name, score = score });

        // Sort descending
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        // Trim list
        if (leaderboard.Count > leaderboardSize)
            leaderboard = leaderboard.GetRange(0, leaderboardSize);

        // Save back to PlayerPrefs
        SaveLeaderboard(leaderboard);
    }

    // Updates the leaderboard UI
    public void ShowLeaderboard()
    {
        panel.SetActive(true);
        List<LeaderboardEntry> leaderboard = LoadLeaderboard();

        // Clear old UI entries
        foreach (Transform child in entriesParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new entries
        for (int i = 0; i < leaderboardSize; i++)
        {
            GameObject entryGO = Instantiate(entryPrefab, entriesParent);
            LeaderboardEntryUI entryUI = entryGO.GetComponent<LeaderboardEntryUI>();
            if (entryUI != null)
            {
                if (i < leaderboard.Count)
                {
                    var entry = leaderboard[i];
                    entryUI.SetData(i + 1, entry.playerName, entry.score);
                }
                else
                {
                    entryUI.SetData(i + 1, "---", 0);
                }
            }
        }
    }

    public void HideLeaderBoard()
    {
        panel.SetActive(false);
    }
    public void ToggleLeaderBoard()
    {
        if (!panel.activeSelf)
        {
            ShowLeaderboard();
        }
        else
        {
        // Toggle between active/inactive
        panel.SetActive(!panel.activeSelf);
        }


    }

    // Loads the leaderboard from PlayerPrefs
    private List<LeaderboardEntry> LoadLeaderboard()
    {
        List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

        for (int i = 0; i < leaderboardSize; i++)
        {
            string entryKey = $"{leaderboardKey}_{i}";
            if (PlayerPrefs.HasKey(entryKey))
            {
                string json = PlayerPrefs.GetString(entryKey);
                LeaderboardEntry entry = JsonUtility.FromJson<LeaderboardEntry>(json);
                leaderboard.Add(entry);
            }
        }

        return leaderboard;
    }

    // Saves the leaderboard to PlayerPrefs
    private void SaveLeaderboard(List<LeaderboardEntry> leaderboard)
    {
        for (int i = 0; i < leaderboardSize; i++)
        {
            string entryKey = $"{leaderboardKey}_{i}";
            if (i < leaderboard.Count)
            {
                string json = JsonUtility.ToJson(leaderboard[i]);
                PlayerPrefs.SetString(entryKey, json);
            }
            else
            {
                PlayerPrefs.DeleteKey(entryKey);
            }
        }

        PlayerPrefs.Save();
    }

    // Updates the score text on UI


    // Optional: Reset all leaderboard data
    public void ResetLeaderboard()
    {
        for (int i = 0; i < leaderboardSize; i++)
        {
            PlayerPrefs.DeleteKey($"{leaderboardKey}_{i}");
        }

        PlayerPrefs.Save();
        ShowLeaderboard();
    }
}
