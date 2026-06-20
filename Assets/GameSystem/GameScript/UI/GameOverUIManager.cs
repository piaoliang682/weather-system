using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance { get; private set; }

    [Header("Game Result UI")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    [HideInInspector]
    public WinUI winUI;
    [HideInInspector]
    public LoseUI loseUI;
    private void Awake()
    {
        // Implement Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        winUI = winPanel.GetComponent<WinUI>();
        loseUI = losePanel.GetComponent<LoseUI>();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void Start()
    {
       winUI.GetExitButton().onClick.AddListener(SystemGameManager.Instance.ReturnLobby);
       winUI.GetRetryButton().onClick.AddListener(SystemGameManager.Instance.RestartGame);
       winUI.GetNextLevelButton().onClick.AddListener(SystemGameManager.Instance.LoadNextLevel);


       loseUI.GetExitButton().onClick.AddListener(SystemGameManager.Instance.ReturnLobby);
       loseUI.GetRetryButton().onClick.AddListener(SystemGameManager.Instance.RestartGame);
       loseUI.GetReviveButton().onClick.AddListener(SystemGameManager.Instance.ReviveGame);
        loseUI.GetLobbyButton().onClick.AddListener(SystemGameManager.Instance.ReturnLobby);
    }

    public void Initiate()
    {
        winUI.SetStarThreholds(GameReference.StarThreholds);
    }

    /// Call this when player wins.
    /// </summary>

    public void ShowGameWin()
    {

        if (winPanel != null) winPanel.SetActive(true);
        if (losePanel != null) losePanel.SetActive(false);

    }

    public void ShowGameLose()
    {

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(true);

    }

    public void HidePanel()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }





}
