using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    private LineRenderer line;

    private WireSocket startSocket;
    private WireSocket endSocket;

    private bool isDragging;

    private Vector3 startPos;
    private Vector3 endPos;
    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    void Start()
    {

    }

    void Update()
    {
        if (isDragging && startSocket != null)
        {
            line.SetPosition(0, startSocket.transform.position);
            line.SetPosition(1, GetMouseWorldPosition());
        }
    }

    public void ConnectToSocket(WireSocket socket)
    {
        if (startSocket == null)
        {
            Debug.Log("start dragging");
            startSocket = socket;
            startPos = socket.transform.position;
            isDragging = true;
            line.enabled = true;

            // Use the socket's material
            MeshRenderer mr = socket.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                line.material = mr.sharedMaterial;
            }
        }
        else
        {

            Debug.Log("end dragging");
            endSocket = socket;
            endPos= socket.transform.position;
            isDragging = false;

            line.SetPosition(0, startSocket.transform.position);
            line.SetPosition(1, endSocket.transform.position);
            WireManager.Instance.ClearActiveWire();
            RegisterToSockets();
        }
    }
    // Call this when the wire finishes connecting
    public void RegisterToSockets()
    {
        if (startSocket != null)
            startSocket.RegisterWire(this);
        if (endSocket != null)
            endSocket.RegisterWire(this);

        // Register to manager
        //WireManager.Instance.RegisterWire(this);
    }
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(
            Camera.main.transform.forward * -1,
            startSocket.transform.position
        );

        if (plane.Raycast(ray, out float dist))
            return ray.GetPoint(dist);

        return startSocket.transform.position;
    }
}
