using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score Display (Optional)")]
    [Tooltip("TMP Text for displaying current score.")]
    [SerializeField] private TMP_Text currentScoreText;

    [Tooltip("TMP Text for displaying high score.")]
    [SerializeField] private TMP_Text highScoreText;

    [Tooltip("TMP Text for displaying high score.")]
    [SerializeField] private TMP_Text rankText;

    [Header("Settings")]
    [Tooltip("The PlayerPrefs key for saving high score.")]
    [SerializeField] private string highScoreKey = "HighScore";

    private int highestScore = 0;
    private int rankNum = 0;
    #region Unity Methods

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadHighScore();
        UpdateUI();
    }

    #endregion

    #region Score Management

    public void AddScore(int amount)
    {
        if (amount <= 0) return;
        GameReference.CurrentScore += amount;
        CheckHighScore();
        UpdateUI();
    }

    public void SubmitScore(string name)
    {
        LeaderboardManager.Instance.SetEntry(name, ((int)GameReference.CurrentScore));
    }

    public void SubmitScore()
    {
        string playerName = PlayerPrefs.GetString(GlobalValue.CURRENT_LOGIN_ACCOUNT_KEY, "玩家");
        LeaderboardManager.Instance.SetEntry(playerName, ((int)GameReference.CurrentScore));
    }

    public void SubtractScore(int amount)
    {
        GameReference.CurrentScore = Mathf.Max(0, GameReference.CurrentScore - Mathf.Abs(amount));
        UpdateUI();
    }

    public void ResetScore()
    {
        GameReference.CurrentScore = 0;
        UpdateUI();
    }

    public bool CheckHighScore()
    {

        if (GameReference.CurrentScore > highestScore)
        {

            highestScore = ((int)GameReference.CurrentScore);
            SaveHighScore();
            return true;
        }
        return false;
    }


    #endregion

    #region Getters and Setters

    public int GetRank() => rankNum;
    public void SetScore(int value)
    {
        GameReference.CurrentScore = Mathf.Max(0, value);
        CheckHighScore();
        UpdateUI();

    }
    public void SetRank(int value)
    {
        rankNum = Mathf.Max(0, value);
        UpdateUI();
    }

    public int GetHighScore() => highestScore;

    public void SetHighScore(int value)
    {
        highestScore = Mathf.Max(highestScore, value);
        SaveHighScore();
        UpdateUI();
    }

    public void ResetHighScore()
    {
        highestScore = 0;
        SaveHighScore();
        UpdateUI();
    }

    #endregion

    #region Save / Load

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(highScoreKey, highestScore);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        highestScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    public bool HasSavedHighScore() => PlayerPrefs.HasKey(highScoreKey);

    public void DeleteSavedHighScore()
    {
        PlayerPrefs.DeleteKey(highScoreKey);
        highestScore = 0;
        UpdateUI();
    }

    #endregion

    #region UI Display

    private void UpdateUI()
    {
        if (currentScoreText != null)
            currentScoreText.text = $"积分: {GameReference.CurrentScore}";

        if (highScoreText != null)
            highScoreText.text = $"最佳: {highestScore}";

        if (rankText != null)
        {
            rankText.text= $"排名: {rankNum}";
        }
    }

    public void SetScoreTextReference(TMP_Text text)
    {
        currentScoreText = text;
        UpdateUI();
    }

    public void SetHighScoreTextReference(TMP_Text text)
    {
        highScoreText = text;
        UpdateUI();
    }

    public void RefreshUI()
    {
        UpdateUI();
    }

    #endregion

    #region Utility

    public void PrintScores()
    {
        Debug.Log($"Current Score: {GameReference.CurrentScore}, High Score: {highestScore}");
    }

    public bool IsNewHighScore() => GameReference.CurrentScore >= highestScore;

    public float GetScorePercentage(int target)
    {
        if (target <= 0) return 0;
        return Mathf.Clamp01((float)GameReference.CurrentScore / target);
    }

    public void AddScoreMultiplier(int baseAmount, float multiplier)
    {
        AddScore(Mathf.RoundToInt(baseAmount * multiplier));
    }

    public void AddRandomScore(int minScore, int maxScore)
    {
        int randomScore = Random.Range(minScore, maxScore + 1);
        AddScore(randomScore);
        Debug.Log($"Added random score: {randomScore}");
    }

    public void AddRandomScore()
    {
        AddRandomScore(10, 100);
    }

    #endregion
}
