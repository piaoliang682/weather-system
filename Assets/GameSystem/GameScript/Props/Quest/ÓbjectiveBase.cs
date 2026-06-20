using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public abstract class ObjectiveBase
{
    public string objectiveID;
    public string description;
    public bool IsCompleted { get; set; }
    public bool IsFailed { get; private set; }


    public virtual void OnStart() 
    {
        IsFailed = false;
        IsCompleted = false;
        // subscribe to event bus
        //EventBus.OnGameEvent += OnGameEvent;
    }
    //public abstract void OnGameEvent(GameEvent e);

    public void CompleteObjective()
    {
        IsCompleted = true;
    }
    public virtual void UpdateProgress(float deltaTime)
    {

    }
    public void FailObjective()
    {
        if (!IsCompleted)
            IsFailed = true;
    }

    public virtual void CleanUp()
    {
        // unsubscribe when no longer needed
        //EventBus.OnGameEvent -= OnGameEvent;
    }

}
