using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }
    public UnityEvent OnCollected;    // invoked when interaction succeeds
    private HashSet<string> collectedItems = new HashSet<string>();
    public Button clueButton;
    public BookPanel cluePanel;
    public BookPanel storyPanel;

    [Header("Collection Goal")]
    [SerializeField] private int collectThreshold = 0;

    public UnityEvent OnAllCollected;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void RecordClue(string itemName)
    {
        if (collectedItems.Add(itemName))
        {
            Debug.Log($"[ClueManager] Collected: {itemName}");
            OnCollected?.Invoke();
            CheckAllCollected();
        }
        else
        {
            Debug.Log($"[ClueManager] Already collected: {itemName}");
        }
    }

    private void CheckAllCollected()
    {


        if (collectThreshold > 0 && collectedItems.Count >= collectThreshold)
        {
            Debug.Log("[ClueManager] All clues collected!");

            OnAllCollected?.Invoke();

        }
    }
    public bool IsItemCollected(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    public HashSet<string> GetCollectedItems()
    {
        return new HashSet<string>(collectedItems);
    }

    public int GetCollectedCount()
    {
        return collectedItems.Count;
    }

    public Button GetClueButton()
    {
        return clueButton;
    }
    public void ClearRecords()
    {
        collectedItems.Clear();
        Debug.Log("[ClueManager] All records cleared");
    }

    public void PrintRecords()
    {
        Debug.Log($"[ClueManager] Total collected items: {collectedItems.Count}");
        foreach (var item in collectedItems)
        {
            Debug.Log($"  - {item}");
        }
    }


    public BookPanel GetStoryPanel()
    {
        return storyPanel;
    }

    public BookPanel GetCluePanel()
    {
        return cluePanel;
    }

    public void CallOnAllCollected()
    {
        Debug.Log("[ClueManager] Manually invoking OnAllCollected event.");
        OnAllCollected?.Invoke();
    }
}
