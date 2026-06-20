using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjective : ObjectiveBase
{
    public float targetScore;     // How long the player must survive (seconds)
    private float currentScore;   // Tracks progress

    private bool isTracking = false;


    public override void OnStart()
    {
        base.OnStart();
        currentScore = 0f;
        isTracking = true;
        GameReference.CurrentScore=0; 
        ScoreManager.Instance.RefreshUI();
    }

    public override void UpdateProgress(float deltaTime)
    {
        if (!isTracking || IsCompleted || IsFailed)
            return;

        //Debug.Log("check");
        currentScore = GameReference.CurrentScore;
        // If player object is missing or dead, fail this objective
        if (GameReference.Player == null || !GameReference.Player.activeSelf)
        {
            FailObjective();
            Debug.LogWarning($"Objective failed due to player death: {objectiveID}");
            return;
        }

        if (currentScore >= targetScore)
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
