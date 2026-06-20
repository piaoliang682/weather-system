using UnityEngine;
using UnityEngine.Events;
public class Inspect : MonoBehaviour
{
    public static Inspect Instance;

    [Header("References")]
    public Camera playerCamera;
    public Transform inspectPoint;

    [Header("Settings")]
    public float rotationSpeed = 200f;
    public float inspectDistance = 3f;


    [Header("Events")]
    public UnityEvent onEnterInspect;
    public UnityEvent onExitInspect;

    private InspectableObject currentObject;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    private bool isInspecting;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isInspecting)
        {
            RotateObject();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopInspect();
            }

            return;
        }

        // Click object to inspect
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, inspectDistance))
            {
                InspectableObject inspectable = hit.collider.GetComponent<InspectableObject>();

                if (inspectable != null)
                {
                    StartInspect(inspectable);
                }
            }
        }
    }

    public void StartInspect(InspectableObject inspectable)
    {
        if (isInspecting)
            return;

        currentObject = inspectable;
        isInspecting = true;

        Transform obj = inspectable.transform;

        originalPosition = obj.position;
        originalRotation = obj.rotation;
        originalParent = obj.parent;

        obj.SetParent(inspectPoint);
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        onEnterInspect?.Invoke();
    }

    public void StopInspect()
    {
        if (currentObject == null)
            return;

        Transform obj = currentObject.transform;

        obj.SetParent(originalParent);
        obj.position = originalPosition;
        obj.rotation = originalRotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        currentObject = null;
        isInspecting = false;

        onExitInspect?.Invoke();
    }

    private void RotateObject()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentObject.transform.Rotate(
            Vector3.up,
            -mouseX * rotationSpeed * Time.deltaTime,
            Space.World);

        currentObject.transform.Rotate(
            inspectPoint.right,
            mouseY * rotationSpeed * Time.deltaTime,
            Space.World);
    }
}