using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<QuestInstance> activeQuests = new List<QuestInstance>();
    private List<QuestInstance> completedQuests = new List<QuestInstance>();
    private List<QuestInstance> allQuests = new List<QuestInstance>(); //this collect all quest

    public QuestDefinition defaultQuest;
    public UnityEvent onQuestFailed;
    public bool isDontDestroyOnLoad = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        activeQuests.Clear();
        completedQuests.Clear();
        allQuests.Clear();
        AddQuest(defaultQuest);
        // Register scene loaded callback
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (activeQuests.Count == 0) return;

        foreach (var quest in activeQuests)
        {
            foreach (var obj in quest.GetObjectives())
            {
                    obj.UpdateProgress(Time.deltaTime);

            }
        }

        CheckQuestStatus();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset or reload your value here
        foreach (var q in activeQuests)
        {
            if (!q.IsGlobal)
            {
                activeQuests.Remove(q);
                AddQuest(q.definition);
            }
        }
        Debug.Log($"Scene {scene.name} loaded - some quest reset");
    }
    public void AddQuest(QuestDefinition def)
    {
        QuestInstance qi = new QuestInstance(def);
        qi.Start();
        activeQuests.Add(qi);
        allQuests.Add(qi);
    }

    private void RemoveQuest(QuestInstance qi)
    {
        completedQuests.Add(qi);
        activeQuests.Remove(qi);
    }

    private void OnQuestCompleted(QuestInstance q)
    {
        Debug.Log($"Quest completed: {q.definition.questID}");
        // Reward logic here, UI, etc.

        RemoveQuest(q);
    }

    private void OnAllQuestComplete()
    {
        SystemGameManager.Instance.GameWin();
    }

    private void OnQuestFailed(QuestInstance q)
    {
        Debug.LogWarning($"Quest failed: {q.definition.questID}");


        SystemGameManager.Instance.GameLose();
        // Failure logic: show UI, penalty, restart, etc.
        RemoveQuest(q);
    }

    public void CheckQuestStatus()
    {
        // Completed quests

        foreach (var q in completedQuests.ToArray())
        {
            OnQuestCompleted(q);
        }


        // Failed quests
        var failed = activeQuests.Where(q => q.IsFailed).ToList();
        foreach (var q in failed)
        {
        Debug.Log("checlk");
            OnQuestFailed(q);
            return;
        }
        if (completedQuests.Count == allQuests.Count)
            OnAllQuestComplete();
    }

    public QuestInstance GetActiveQuestByID(string questID)
    {
        return activeQuests.FirstOrDefault(q => q.definition.questID == questID);
    }

    public void CompleteObjective(string objID)
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            activeQuests[i].MarkObjectiveComplete(objID);
        }

        CheckQuestStatus();
    }

    public void FailObjective(string objID)
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {
            activeQuests[i].MarkObjectiveFailed(objID);
        }

        CheckQuestStatus();
    }
}
