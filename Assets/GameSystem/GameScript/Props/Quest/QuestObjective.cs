using System;
using System.Collections.Generic;
using UnityEngine;

// Base interface for any kind of objective
public interface IQuestObjective
{
    bool IsCompleted { get; }
    void OnStart();
    void OnEvent(string eventType, object eventData);
    // eventType / eventData are generic ¡ª you design your own event system
}