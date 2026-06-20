using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private List<GameObject> stars; // star GameObjects to activate
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button homeButton;

    [Header("Star Thresholds")]
    [Tooltip("Minimum score required for each star (ascending order)")]
    private List<int> starThresholds = new List<int> { 100, 200, 300 };

    private int currentScore;

    private void Awake()
    {

    }
    void Start()
    {
        // Example: get score from GameManager if you have one
        // currentScore = GameManager.Instance.Score;
        currentScore = ((int)GameReference.CurrentScore);

        UpdateUI();
    }

    public void SetStarThreholds(List<int> threholds)
    {
        starThresholds = threholds;
    }
    private void UpdateUI()
    {
        // Update score display
        if (scoreText != null)
            scoreText.text = currentScore.ToString();

        // Activate stars based on thresholds
        int starsEarned = 0;
        for (int i = 0; i < starThresholds.Count; i++)
        {
            if (currentScore >= starThresholds[i])
                starsEarned++;
        }

        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(i < starsEarned);
        }
    }
    public Button GetNextLevelButton()
    {
        return nextLevelButton;
    }
    public Button GetExitButton()
    {
        return homeButton;
    }
    public Button GetRetryButton()
    {
        return retryButton;
    }

}
