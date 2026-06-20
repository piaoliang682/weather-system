using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
public class LockPick : MonoBehaviour
{
    public GameObject gameRoot;
    public Camera cam;
    public Transform innerLock;
    public Transform pickPosition;
    public bool isOnce = true;
    public float maxAngle = 90f;
    public float lockSpeed = 10f;


    public UnityEvent onUnlock;
    [Range(1, 25)]
    public float lockRange = 10f;

    private float eulerAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    private float keyPressTime = 0f;
    private bool movePick = true;
    private Vector3 rotationAxis;



    public bool isStart = false;
    void Start()
    {
        newLock();
        rotationAxis = pickPosition.forward; // or pickPosition.up depending on your setup
    }

    void Update()
    {
        if (isStart){
            HandlePickMovement();
            HandleLocking();
            HandleInput();
        }

    }

    public void CallStart()
    {
        isStart = true;
    }
    // ---------------------------
    // PICK MOVEMENT (stable)
    // ---------------------------
    void HandlePickMovement()
    {
        // lock pick position to anchor
        transform.position = pickPosition.position;

        if (!movePick) return;

        Vector3 mouseLocal = GetMousePointOnLockPlaneLocal();

        Vector3 dir = mouseLocal - pickPosition.localPosition;

        //Debug.Log(dir);

        eulerAngle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0, 0, eulerAngle);
    }

    // ---------------------------
    // INPUT
    // ---------------------------
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            movePick= false;
            keyPressTime = 1f;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            movePick = true;
            keyPressTime = 0f;
        }
    }

    // ---------------------------
    // LOCK MECHANIC
    // ---------------------------
    void HandleLocking()
    {
        float distanceFromTarget = Mathf.Abs(eulerAngle - unlockAngle);

        float percentage = Mathf.Clamp01(1f - (distanceFromTarget / 100f)) * 100f;

        float lockRotation = (percentage / 100f) * maxAngle * keyPressTime;
        float maxRotation = (percentage / 100f) * maxAngle;

        // smooth inner lock rotation WITHOUT jitter
        float current = innerLock.localEulerAngles.z;
        if (current > 180f) current -= 360f;

        float lockLerp = Mathf.Lerp(current, lockRotation, Time.deltaTime * lockSpeed);

        innerLock.localEulerAngles = new Vector3(0, 0, lockLerp);

        // unlock check
        if (!movePick && Mathf.Abs(lockLerp - maxRotation) < 1f)
        {
            if (eulerAngle >= unlockRange.x && eulerAngle <= unlockRange.y)
            {
                Debug.Log("Unlocked!");
                onUnlock?.Invoke();
                if (isOnce)
                    gameRoot.SetActive(false);
                newLock();
                movePick = true;
                keyPressTime = 0f;
            }
        }
    }

    // ---------------------------
    // NEW LOCK SETUP
    // ---------------------------
    void newLock()
    {
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }

    // ---------------------------
    // MOUSE PLANE (stable input)
    // ---------------------------
    Vector3 GetMousePointOnLockPlaneLocal()
    {
        Plane plane = new Plane(-cam.transform.forward, pickPosition.position);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 worldPoint = ray.GetPoint(enter);
            //Debug.Log(pickPosition.InverseTransformPoint(worldPoint));
            return pickPosition.InverseTransformPoint(worldPoint);

        }

        return Vector3.zero;
    }
}