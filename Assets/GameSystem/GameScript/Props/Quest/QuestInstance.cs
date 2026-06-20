

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

// The runtime instance of a quest ¡ª tracks progress separately from the definition
public class QuestInstance
{
    public QuestDefinition definition;
    private List<ObjectiveBase> objectives;

    public bool IsCompleted => objectives.All(o => o.IsCompleted);
    public bool IsFailed => objectives.Any(o => o.IsFailed);
    public bool IsGlobal { get; set; }

    public QuestInstance(QuestDefinition def)
    {
        definition = def;
        objectives = new List<ObjectiveBase>();
        IsGlobal = def.isGlobal;
        foreach (var defObj in def.objectives)
        {
            objectives.Add(defObj);
        }
    }

    public void Start()
    {
        foreach (var obj in objectives)
        {
            obj.OnStart();
        }
    }

    public void CleanUp()
    {
        foreach (var obj in objectives)
        {
            if (obj is ObjectiveBase baseObj)
                baseObj.CleanUp();
        }
    }
    public ObjectiveBase GetObjectiveByID(string objID)
    {
        // assuming your objective has an ID; otherwise you could use index
        return objectives.FirstOrDefault(o => o.objectiveID == objID);
    }

    public ObjectiveBase GetUncompletedObjectiveByID(string objID)
    {
        return objectives.FirstOrDefault(o => o.objectiveID == objID && !o.IsCompleted);
    }

    // New: mark objective complete (by ID or index)
    public bool MarkObjectiveComplete(string objID)
    {
        var obj = GetUncompletedObjectiveByID(objID);
        if (obj != null)
        {
            obj.CompleteObjective();
            Debug.Log(obj.description);
            QuestManager.Instance.CheckQuestStatus();
            return true;

        }
        QuestManager.Instance.CheckQuestStatus();
        return false;
    }

        public bool MarkObjectiveFailed(string objID)
    {
        var obj = GetUncompletedObjectiveByID(objID);
        if (obj != null)
        {
            obj.FailObjective();
            Debug.Log($"Objective failed: {obj.description}");

            // Quest might fail now
            QuestManager.Instance.CheckQuestStatus();
            return true;
        }

        QuestManager.Instance.CheckQuestStatus();
        return false;
    }
    
    public List<ObjectiveBase> GetObjectives()
    {
        return objectives;
    }
}