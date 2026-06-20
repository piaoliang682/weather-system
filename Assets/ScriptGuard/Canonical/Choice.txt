using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Choice : MonoBehaviour
{
    [Header("List of Options")]
    public GameObject[] options;  // assign GameObjects in inspector
    public UnityEvent onChoose;
    /// <summary>
    /// Activate the option at index and deactivate all others.
    /// </summary>
    public void Choose(int index)
    {
        if (options == null || options.Length == 0)
        {
            Debug.LogWarning("No options assigned!");
            return;
        }

        if (index < 0 || index >= options.Length)
        {
            Debug.LogWarning("Index out of range!");
            return;
        }

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] != null)
                options[i].SetActive(i == index);
        }
        onChoose?.Invoke();
    }

    /// <summary>
    /// Activate a choice by GameObject reference.
    /// </summary>
    public void Choose(GameObject choice)
    {
        if (options == null || options.Length == 0) return;

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] != null)
                options[i].SetActive(options[i] == choice);
        }
    }

    /// <summary>
    /// Deactivate all options.
    /// </summary>
    public void DeactivateAll()
    {
        if (options == null) return;
        foreach (var obj in options)
            if (obj != null)
                obj.SetActive(false);
    }
}
