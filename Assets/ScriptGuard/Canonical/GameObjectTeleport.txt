using UnityEngine;

public class GameObjectTeleport : MonoBehaviour
{
    [Header("Teleport")]
    public Transform objectToTeleport;
    public Transform destination;

    public void Teleport()
    {
        if (objectToTeleport == null || destination == null)
        {
            Debug.LogWarning("Teleport failed: Missing object or destination.");
            return;
        }

        objectToTeleport.position = destination.position;
        objectToTeleport.rotation = destination.rotation;
    }
}