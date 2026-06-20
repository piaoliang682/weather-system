using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 3f;
    public Camera playerCamera;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // Check if the hit object or one of its parents has a DoorController
            DoorController door = hit.collider.GetComponentInParent<DoorController>();

            if (door != null)
            {
                door.ToggleDoor();
            }
        }
    }
}