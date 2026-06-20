using UnityEngine;
using UnityEngine.Events;

public class EnterCameraViewTrigger : MonoBehaviour
{
    [Header("Player")]
    private Transform player;

    [Header("Interaction")]
    public float interactDistance = 5f;
    public UnityEvent onInteractSuccess;    // invoked when interaction succeeds
    public UnityEvent OnExit;    // invoked when interaction succeeds
    [Header("Object Camera")]
    public Camera targetCamera;

    [Header("Book Panel")]
    public BookPanel bookPanel;
    public int bookPageIndex = 0;

    [Header("Optional")]
    public GameObject[] disableObjects;
    public MonoBehaviour[] disableScripts;

    private bool isActive = false;

    private void Start()
    {
        player = GameReference.Player.transform;

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }
    }
    private void OnDisable()
    {
        Debug.Log("CameraSwitcher disabled, exiting camera view if active.");
        ExitCameraView();

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }

        // =========================
        // OPTIONAL: PRESS ESC TO EXIT CAMERA
        // =========================
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitCameraView();
        }
        
    }

    private void TryInteract()
    {
        if (player == null || targetCamera == null)
            return;
        Ray ray;
        Camera cam = Camera.main;
        if (cam == null) return;
        
            ray = cam.ScreenPointToRay(Input.mousePosition);
        
         

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform != transform)
                return;

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist > interactDistance)
            {
                Debug.Log("Too far away");
                return;
            }

            ToggleCamera();
        }
        else
        {
                       Debug.Log("No hit detected when trying to interact with camera switcher: " + gameObject.name);
        }
    }

    // =========================
    // TOGGLE CAMERA
    // =========================
    private void ToggleCamera()
    {
        Debug.Log("Toggling camera view: " + (isActive ? "OFF" : "ON"));
        isActive = !isActive;

        ApplyCameraState(isActive);

        // Call BookPanel on interaction success
        if (bookPanel != null)
        {
            bookPanel.ShowBookPanel(bookPageIndex);
        }

        onInteractSuccess?.Invoke();
    }

    // =========================
    // EXIT CAMERA (FORCE BACK TO MAIN)
    // =========================
    public void ExitCameraView()
    {
        if (bookPanel!=null)
            bookPanel.HideBookPanel();
        if (!isActive) return;

        isActive = false;

        ApplyCameraState(false);

    }

    // =========================
    // CENTRAL CAMERA LOGIC
    // =========================
    private void ApplyCameraState(bool active)
    {
        // CAMERA SWITCH
        if (CameraGroupManager.Instance != null)
        {
            if (active)
                CameraGroupManager.Instance.SetActiveCamera(targetCamera);
            else
                CameraGroupManager.Instance.ResetToDefault();
        }

        // DISABLE OBJECTS
        foreach (GameObject obj in disableObjects)
        {
            if (obj != null)
                obj.SetActive(!active);
        }

        // DISABLE SCRIPTS
        foreach (MonoBehaviour script in disableScripts)
        {
            if (script != null)
                script.enabled = !active;
        }
    }
}