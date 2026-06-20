using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ARPGCamera : MonoBehaviour
{
    public RectTransform blockArea;

    public Transform target;
    public float targetHeight = 1.2f, targetSide = -0.15f;
    public float distance = 4f, minDistance = 1f, maxDistance = 6f;
    public float xSpeed = 1, ySpeed = 1;
    public float yMinLimit = -10f, yMaxLimit = 70f;
    public float zoomRate = 80f;

    private float x = 20f, y = 0f;
    private Camera cam;

    void Awake()
    {
        GameReference.CameraTransform = transform;
        cam = GetComponent<Camera>();
        if (!target) target = GameObject.FindWithTag("Player")?.transform;
        Vector3 angles = transform.eulerAngles;
        x = angles.y; y = angles.x;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    void Update()
    {
        if (!target || Time.timeScale == 0) return;

        if (!IsPointerOverRect())
        {
            x += Input.GetAxis("Mouse X") * xSpeed;
            y -= Input.GetAxis("Mouse Y") * ySpeed;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomRate * Mathf.Abs(distance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }
    void LateUpdate()
    {
        if (!target) return;

        Quaternion rot = Quaternion.Euler(y, x, 0f);
        transform.rotation = rot;

        Vector3 desired = target.position
            - (rot * new Vector3(targetSide, 0, 1) * distance
            + new Vector3(0, -targetHeight, 0));

        Vector3 origin = target.position - new Vector3(targetSide, -targetHeight, 0);

        transform.position = AdjustForObstacles(origin, desired);
    }


    Vector3 AdjustForObstacles(Vector3 from, Vector3 to)
    {
        if (Physics.Linecast(from, to, out var hit) && hit.transform.CompareTag("Wall"))
            return hit.point + hit.normal * 0.1f;
        return to;
    }

    static float ClampAngle(float ang, float min, float max)
    {
        if (ang < -360f) ang += 360f;
        if (ang > 360f) ang -= 360f;
        return Mathf.Clamp(ang, min, max);
    }

    private bool IsPointerOverRect()
    {
        if (blockArea == null)
            return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            blockArea,
            Input.mousePosition,
            null
        );
    }

}
