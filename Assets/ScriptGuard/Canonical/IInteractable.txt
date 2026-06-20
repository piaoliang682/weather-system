using UnityEngine;

// Define the interaction interface
public interface IInteractable
{
    // Method to be called when interacting
    void HandleInteraction(GameObject interactor);
}