using UnityEngine;

public class WaterCan : MonoBehaviour
{
    [Header("Drag")]
    public float dragZDepth = 0f;

    [Header("Pouring")]
    public float pourAngle = -45f;
    public float rotateSpeed = 10f;

    [Header("Water Effect")]
    public GameObject waterEffect;

    private bool isDragging;
    private Vector3 offset;
    private Quaternion originalRotation;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        originalRotation = transform.rotation;

        if (waterEffect != null)
            waterEffect.SetActive(false);
    }

    private void OnMouseDown()
    {
        isDragging = true;

        Vector3 mouseWorld = GetMouseWorldPos();
        offset = transform.position - mouseWorld;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        // Move
        transform.position = GetMouseWorldPos() + offset;

        // Rotate to pouring angle
        Quaternion targetRot = Quaternion.Euler(0, 0, pourAngle);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotateSpeed
        );

        // Enable water
        if (waterEffect != null && !waterEffect.activeSelf)
            waterEffect.SetActive(true);
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Disable water
        if (waterEffect != null)
            waterEffect.SetActive(false);
    }

    private void Update()
    {
        if (!isDragging)
        {
            // Return to original rotation
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                originalRotation,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(cam.transform.position.z) + dragZDepth;
        return cam.ScreenToWorldPoint(mouse);
    }
}
