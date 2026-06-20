using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    // Start is called before the first frame update
    IInteractable currentInteractable;
    public void HandleInteraction()
    {
        if (currentInteractable == null)
        {
            // either never set, or the object was destroyed
            Debug.Log("No interactable to handle");
            return;
        }

        // Try to cast to a MonoBehaviour if possible
        var mb = currentInteractable as MonoBehaviour;
        if (mb != null)
        {
            // Check if the GameObject is active in the scene
            if (!mb.gameObject.activeInHierarchy)
            {
                Debug.Log("Interactable GameObject is not active");
                return;
            }

            // Check if the behaviour itself is enabled
            if (!mb.isActiveAndEnabled)
            {
                Debug.Log("Interactable component is disabled");
                return;
            }
        }

        // Now it's (relatively) safe to interact
        currentInteractable.HandleInteraction(gameObject);
    }
    public void SetCurrentInteractable(IInteractable i)
    {
        currentInteractable = i;
    }

}
