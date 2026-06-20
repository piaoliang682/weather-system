using UnityEngine;
using UnityEngine.UI;

public class CluesObject : MonoBehaviour
{
    private string clueName;
    public int clueIndex;
    private void OnMouseDown()
    {

        Activate();
    }

    public void Activate()
    {
        clueName=gameObject.name;
        if (ClueManager.Instance != null)
        {
            Button btn = ClueManager.Instance.GetClueButton();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnCollected);

            Debug.Log($"[CluesObject] {gameObject.name} activated");
        }
        else
        {
            Debug.LogWarning("[CluesObject] ClueManager not found");
        }

        if (ClueManager.Instance.GetCluePanel() != null)
        {
            ClueManager.Instance.GetCluePanel().ShowBookPanel(clueIndex);
        }
        else
        {
            Debug.Log("ClueManager null");
        }
    }

    public void OnCollected()
    {
        if (ClueManager.Instance != null)
        {
            ClueManager.Instance.RecordClue(gameObject.name);
            Debug.Log($"[CluesObject] Collected clue: {gameObject.name}");
        }
        if (ClueManager.Instance.GetStoryPanel() != null)
        {
            ClueManager.Instance.GetStoryPanel().ShowBookPanel(clueIndex);
            ClueManager.Instance.GetStoryPanel().ShowTemp(8f);
        }
        if (ClueManager.Instance.GetCluePanel() != null)
        {
            ClueManager.Instance.GetCluePanel().HideBookPanel();
        }
        gameObject.SetActive(false);

        Debug.Log($"[CluesObject] {gameObject.name} deactivated");
    }
}