using System;
using UnityEngine;
 
public class SurvivalObjective : ObjectiveBase
{
    public float durationToSurvive;     // How long the player must survive (seconds)
    private float currentSurvivalTime;   // Tracks progress

    private bool isTracking = false;


    public override void OnStart()
    {
        base.OnStart();
        currentSurvivalTime = 0f;
        isTracking = true;
    }

    public override void UpdateProgress(float deltaTime)
    {
        if (!isTracking || IsCompleted || IsFailed)
            return;

        // If player object is missing or dead, fail this objective
        if (GameReference.Player == null)
        {
            FailObjective();
            Debug.LogWarning($"Objective failed due to player death: {objectiveID}");
            return;
        }

        currentSurvivalTime += deltaTime;

        if (currentSurvivalTime >= durationToSurvive)
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
