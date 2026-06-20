using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationObjective : ObjectiveBase
{
    public int targetCount;     // How long the player must survive (seconds)
    private int currentCount;   // Tracks progress

    private bool isTracking = false;


    public override void OnStart()
    {
        base.OnStart();
        GameReference.EnemyEliminationCount=0;
        ScoreManager.Instance.RefreshUI();
        currentCount = 0;
        isTracking = true;
    }

    public override void UpdateProgress(float deltaTime)
    {
        currentCount = GameReference.EnemyEliminationCount;
        if (!isTracking || IsCompleted || IsFailed)
            return;

        // If player object is missing or dead, fail this objective
        if (GameReference.Player == null)
        {
            FailObjective();
            Debug.LogWarning($"Objective failed due to player death: {objectiveID}");
            return;
        }

        if (currentCount >= targetCount)
        {
            CompleteObjective();
            Debug.Log($"Survival objective completed: {objectiveID}");
        }
    }

    public override void CleanUp()
    {
        base.CleanUp();
        isTracking = false;
    }
}
