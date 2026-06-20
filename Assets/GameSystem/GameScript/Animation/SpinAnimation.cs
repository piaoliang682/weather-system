using UnityEngine;

public class SpinAnimation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float spinSpeed = 100f; // Speed of rotation in degrees per second

    [Header("Axis Settings")]
    public bool rotateOnX = false; // Enable rotation on X-axis
    public bool rotateOnY = false; // Enable rotation on Y-axis
    public bool rotateOnZ = false; // Enable rotation on Z-axis

    [Header("Play Settings")]
    public bool playOnStart = true; // Should the rotation start on play

    private bool isRotating;
    private Quaternion originalRotation; // Store the original rotation

    private void Start()
    {
        // Store the initial rotation of the object
        originalRotation = transform.rotation;

        // Start the rotation automatically if playOnStart is true
        if (playOnStart)
        {
            isRotating = true;
        }
    }

    private void Update()
    {
        // Only rotate if isRotating is true
        if (isRotating)
        {
            // Calculate rotation values for each axis
            float xRotation = rotateOnX ? spinSpeed * Time.deltaTime : 0f;
            float yRotation = rotateOnY ? spinSpeed * Time.deltaTime : 0f;
            float zRotation = rotateOnZ ? spinSpeed * Time.deltaTime : 0f;

            // Apply rotation to the object
            transform.Rotate(xRotation, yRotation, zRotation);
        }
    }

    /// <summary>
    /// Starts the spin animation if it's not already rotating.
    /// </summary>
    public void StartRotation()
    {
        isRotating = true;
    }

    /// <summary>
    /// Stops the spin animation and returns the object to its original rotation.
    /// </summary>
    public void StopRotation()
    {
        isRotating = false;
        transform.rotation = originalRotation; // Reset to original rotation
    }
}
