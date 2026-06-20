using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Game/Quest/QuestDefinition", order = 1)]
public class QuestDefinition : ScriptableObject
{
    [Tooltip("this is used to serialise buffeffect polymorphism by name, change name for each type of effect and click initialise")]
    public string initialiseName;

    [Header("Quest Info")]
    public string questID;
    public string title;
    [TextArea] public string description;

    [Header("Objectives")]
    [SerializeReference]
    public List<ObjectiveBase> objectives = new List<ObjectiveBase>();

    public bool isGlobal = false;



    // You can add more fields: prerequisites, rewards, branching info, etc.

    public void Initialise()
    {
        // Search all assemblies for the class
        Type type = Type.GetType(initialiseName);
        if (type == null)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(initialiseName);
                if (type != null) break;
            }
        }

        if (type == null)
        {
            Debug.LogError($"BuffSO: Could not find class '{initialiseName}'. Make sure it's a subclass of BuffBase.");
            return;
        }

        if (!typeof(ObjectiveBase).IsAssignableFrom(type))
        {
            Debug.LogError($"BuffSO: Class '{initialiseName}' is not a BuffBase.");
            return;
        }

        ObjectiveBase newEffect = (ObjectiveBase)Activator.CreateInstance(type);
        objectives.Add(newEffect);
    }
}
