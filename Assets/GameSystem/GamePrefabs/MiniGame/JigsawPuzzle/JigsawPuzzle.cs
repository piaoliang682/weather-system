using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class JigsawPuzzle : MonoBehaviour
{
    // =========================
    // SELECTED PIECE
    // =========================
    private GameObject selectedObject;

    // =========================
    // PIECES
    // =========================
    private GameObject[] dragObj;

    // =========================
    // DROP POINTS (TARGETS)
    // =========================
    private GameObject[] dropObj;

    public GameObject endPanel;

    public static JigsawPuzzle Instance;

    [Header("Events")]
    public UnityEvent onPuzzleComplete;

    // =========================
    // PUZZLE ROOT (IMPORTANT)
    // shared parent for drop + pieces
    // =========================
    [Header("Puzzle Root (Shared Parent)")]
    [SerializeField] private Transform puzzlePlane;

    // ray height offset above plane
    [SerializeField] private float planeOffset = 0.01f;
    [SerializeField] private float snapThreshold;
    // visual lift to avoid overlap
    [Header("Drop Height Offset")]
    [SerializeField] private float dropYOffset = 0.01f;

    // optional camera override
    [Header("Camera (Optional)")]
    [SerializeField] private Camera cam;

    private void Awake()
    {
        Instance = this;

        // fallback puzzle root
        if (puzzlePlane == null)
            puzzlePlane = transform;

        // fallback camera
        if (cam == null)
            cam = Camera.main;
    }

    void Start()
    {
        dragObj = GameObject.FindGameObjectsWithTag("drag");
        dropObj = GameObject.FindGameObjectsWithTag("drop");
    }

    // =========================
    // WIN CHECK
    // =========================
    private bool IsFinish()
    {
        foreach (var item in dragObj)
        {
            JigsawPuzzlePiece piece = item.GetComponent<JigsawPuzzlePiece>();

            if (piece != null && !piece.isTrueDown)
                return false;
        }

        return true;
    }

    void Update()
    {
        // =========================
        // LEFT CLICK
        // =========================
        if (Input.GetMouseButtonDown(0))
        {
            // =========================
            // PICK PIECE
            // =========================
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null && hit.collider.CompareTag("drag"))
                {
                    selectedObject = hit.collider.gameObject;

                    Cursor.visible = false;

                    JigsawPuzzlePiece piece =
                        selectedObject.GetComponent<JigsawPuzzlePiece>();

                    if (piece != null)
                        piece.isTrueDown = false;
                }
            }
            // =========================
            // DROP PIECE
            // =========================
            else
            {
                Vector3 worldPosition = GetMousePlanePosition();

                Transform parent = puzzlePlane;

                // convert mouse to LOCAL space
                Vector3 localMousePos = parent.InverseTransformPoint(worldPosition);

                Vector3 nearestDropLocal = Vector3.zero;
                float minDistance = 10f;
                string selectName = "";

                // =========================
                // FIND NEAREST DROP (LOCAL SPACE)
                // =========================
                foreach (var item in dropObj)
                {
                    Vector3 localDropPos =
                        parent.InverseTransformPoint(item.transform.position);

                    float dist = Vector3.Distance(localDropPos, localMousePos);
                    Debug.Log($"[DROP CHECK] {localMousePos} at {localDropPos}, distance = {dist:F3}");
                    if (dist <= minDistance)
                    {
                        minDistance = dist;
                        nearestDropLocal = localDropPos;
                        selectName = item.name;
                    }
                }

                // =========================
                // SNAP SYSTEM
                // =========================
                if (minDistance < snapThreshold)
                {
                    Debug.Log($"[SNAP] Attempted snap to: {selectName}, distance = {minDistance:F3}");

                    // convert back to world position
                    Vector3 worldSnap =
                        parent.TransformPoint(nearestDropLocal);

                    selectedObject.transform.position =
                        worldSnap + puzzlePlane.up * dropYOffset;

                    // correct slot check
                    if (selectedObject.name == selectName)
                    {
                        Debug.Log($"[SNAP SUCCESS] Correct piece: {selectedObject.name} ¡ú {selectName}");

                        JigsawPuzzlePiece piece =
                            selectedObject.GetComponent<JigsawPuzzlePiece>();

                        if (piece != null)
                            piece.isTrueDown = true;

                        if (IsFinish())
                        {
                            Debug.Log("[PUZZLE COMPLETE] All pieces placed correctly!");

                            if (endPanel != null)
                                endPanel.SetActive(true);

                            onPuzzleComplete?.Invoke();
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[SNAP WRONG] Piece {selectedObject.name} snapped to {selectName}");
                    }
                }
                else
                {
                    Debug.Log($"[NO SNAP] Distance too far: {minDistance:F3}");

                    selectedObject.transform.position = worldPosition;
                }

                selectedObject = null;
                Cursor.visible = true;
            }
        }

        // =========================
        // DRAGGING
        // =========================
        if (selectedObject != null)
        {
            Vector3 worldPosition = GetMousePlanePosition();

            selectedObject.transform.position = worldPosition;

            if (Input.GetMouseButtonDown(1))
            {
                RotatePiece();
            }
        }
    }

    // =========================
    // CAMERA SAFE
    // =========================
    private Camera GetCamera()
    {
        return cam != null ? cam : Camera.main;
    }

    public void FinishPuzzle()
    {
        onPuzzleComplete?.Invoke();
    }

    // =========================
    // MOUSE ON PLANE
    // =========================
    private Vector3 GetMousePlanePosition()
    {
        Camera activeCam = GetCamera();

        if (activeCam == null)
            return Vector3.zero;

        Plane plane = new Plane(
            puzzlePlane.up,
            puzzlePlane.position + puzzlePlane.up * planeOffset
        );

        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    // =========================
    // ROTATE PIECE
    // =========================
    private void RotatePiece()
    {
        selectedObject.transform.Rotate(
            puzzlePlane.up,
            90f,
            Space.World
        );
    }

    // =========================
    // RAYCAST
    // =========================
    private RaycastHit CastRay()
    {
        Camera activeCam = GetCamera();

        RaycastHit hit = new RaycastHit();

        if (activeCam == null)
            return hit;

        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit);

        return hit;
    }

#if UNITY_EDITOR
    // =========================
    // DEBUG VISUAL
    // =========================
    private void OnDrawGizmos()
    {
        if (puzzlePlane == null)
            return;

        Gizmos.color = Color.green;

        Vector3 center =
            puzzlePlane.position + puzzlePlane.up * planeOffset;

        Gizmos.DrawRay(center, puzzlePlane.up);
    }
#endif
}