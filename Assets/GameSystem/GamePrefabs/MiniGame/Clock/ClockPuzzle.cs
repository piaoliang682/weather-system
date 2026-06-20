using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ClockPuzzle : MonoBehaviour
{
    public Camera cam;
    public Transform hourNeedle;             // hour needle
    public Transform minuteNeedle;           // minute needle
    public Transform clockCenter;
    public TMP_Text resultText;              // display results

    [SerializeField]
    public UnityEvent onPuzzleSolved = new UnityEvent();

    [Range(1, 180)]
    public float snapRange = 10f;

    public float rotateSpeed = 10f;

    private float hourAngle;                 // angle for hour needle
    private float hourTargetAngle;           // target for hour needle
    private float minuteAngle;               // angle for minute needle
    private float minuteTargetAngle;         // target for minute needle

    private float previousHourAngle;         // previous hour angle for delta calculation
    private float previousMinuteAngle;       // previous minute angle for delta calculation

    private int activeNeedle = 1;            // which needle is being dragged (1 or 2)
    private bool dragging = false;

    public bool isStart = false;

    void Start()
    {
        // Initialize angles from needle transforms
        hourAngle = hourNeedle.localRotation.eulerAngles.z;
        minuteAngle = minuteNeedle.localRotation.eulerAngles.z;

        // Initialize previous angles with current angles
        previousHourAngle = hourAngle;
        previousMinuteAngle = minuteAngle;
        GenerateTargets();
    }

    void Update()
    {
        if (!isStart) return;

        HandleInput();

        // Debug: Log dragging status
        //if (dragging && Time.frameCount % 30 == 0)
        //{
        //    Debug.Log($"[ClockPuzzle] ◉ DRAGGING ACTIVE - Needle {(activeNeedle == 1 ? "HOUR" : "MINUTE")}, Mouse: {Input.mousePosition}");
        //}

        HandleRotation();
    }

    public void CallStart()
    {
        isStart = true;
    }

    // ---------------------------
    // INPUT
    // ---------------------------
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            dragging = true;
            Debug.Log($"[ClockPuzzle] ▼ DRAG STARTED - Needle {activeNeedle}, Mouse Position: {Input.mousePosition}");
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            dragging = false;
            Debug.Log($"[ClockPuzzle] ▲ DRAG ENDED - Needle {activeNeedle}, Final Position: {Input.mousePosition}");
            CheckSolution();
        }

        // Switch between needles with E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeNeedle = activeNeedle == 1 ? 2 : 1;
            Debug.Log("[ClockPuzzle] ◄► Switched to needle " + (activeNeedle == 1 ? "HOUR" : "MINUTE"));
        }
    }

    // ---------------------------
    // ROTATION (RELATIVE, ANY ANGLE)
    // ---------------------------
    void HandleRotation()
    {
        if (!dragging) 
        {
            // Still apply the rotations to maintain needle positions
            if (hourNeedle != null)
                hourNeedle.localRotation = Quaternion.Euler(0, 0, hourAngle);
            if (minuteNeedle != null)
                minuteNeedle.localRotation = Quaternion.Euler(0, 0, minuteAngle);
            return;
        }

        if (clockCenter == null)
        {
            Debug.LogError("[ClockPuzzle] ✗ DRAG ERROR: Clock Center is not assigned!");
            return;
        }

        if (cam == null)
        {
            Debug.LogError("[ClockPuzzle] ✗ DRAG ERROR: Camera is not assigned!");
            return;
        }

        // get mouse on lock plane (WORLD)
        Vector3 mouseWorld = GetMousePointOnPlane();

        // convert to LOCAL space of clock center
        Vector3 localPoint = clockCenter.InverseTransformPoint(mouseWorld);

        // IMPORTANT: stable 2D angle
        float angle = Mathf.Atan2(-localPoint.x, localPoint.y) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, -180f, 180f);

        // Update the active needle
        if (activeNeedle == 1)
        {
            // Only update previousHourAngle if it hasn't been set (first drag)
            if (previousHourAngle == hourAngle)
                previousHourAngle = hourAngle;

            hourAngle = angle;
            float angleDelta = Mathf.DeltaAngle(previousHourAngle, hourAngle);
            previousHourAngle = hourAngle;

            hourNeedle.localRotation = Quaternion.Euler(0, 0, hourAngle);
            minuteNeedle.localRotation = Quaternion.Euler(0, 0, minuteAngle);

            // Log angle movement - detailed logging every frame during dragging
            //Debug.Log($"[ClockPuzzle] ⟲ ROTATING HOUR: Angle {Quaternion.Euler(0, 0, hourAngle):F1}° | Δ {angleDelta:F1}° | LocalPoint ({localPoint.x:F2}, {localPoint.y:F2}, {localPoint.z:F2}) | Time {AngleToHour12(hourAngle):D2}:{AngleToMinute(minuteAngle):D2}");
        }
        else
        {
            // Only update previousMinuteAngle if it hasn't been set (first drag)
            if (previousMinuteAngle == minuteAngle)
                previousMinuteAngle = minuteAngle;

            minuteAngle = angle;
            float angleDelta = Mathf.DeltaAngle(previousMinuteAngle, minuteAngle);
            previousMinuteAngle = minuteAngle;

            hourNeedle.localRotation = Quaternion.Euler(0, 0, hourAngle);
            minuteNeedle.localRotation = Quaternion.Euler(0, 0, minuteAngle);

            // Log angle movement - detailed logging every frame during dragging
            //Debug.Log($"[ClockPuzzle] ⟲ ROTATING MINUTE: Angle {Quaternion.Euler(0, 0, minuteAngle):F1}° | Δ {angleDelta:F1}° | LocalPoint ({localPoint.x:F2}, {localPoint.y:F2}, {localPoint.z:F2}) | Time {AngleToHour12(hourAngle):D2}:{AngleToMinute(minuteAngle):D2}");
        }

        UpdateDisplay();
    }

    // ---------------------------
    // CHECK PUZZLE SOLVE
    // ---------------------------
    void CheckSolution()
    {
        // Only check solution when not dragging (when mouse is released)
        if (dragging)
            return;

        float diff1 = Mathf.Abs(Mathf.DeltaAngle(hourAngle, hourTargetAngle));
        float diff2 = Mathf.Abs(Mathf.DeltaAngle(minuteAngle, minuteTargetAngle));

        int currentHour = AngleToHour12(hourAngle);
        int targetHour = AngleToHour12(hourTargetAngle);
        int currentMinute = AngleToMinute(minuteAngle);
        int targetMinute = AngleToMinute(minuteTargetAngle);

        Debug.Log($"[ClockPuzzle] CHECK SOLUTION - Current: {currentHour:D2}:{currentMinute:D2} | Target: {targetHour:D2}:{targetMinute:D2} | Hour Diff: {diff1:F2}° | Minute Diff: {diff2:F2}° | Snap Range: {snapRange}°");

        // Both needles must be within snap range
        if (diff1 <= snapRange && diff2 <= snapRange)
        {
            Debug.Log("Clock Solved!");
            DisplayResult();
            onPuzzleSolved.Invoke();
            GenerateTargets();
        }
    }

    // ---------------------------
    // NEW TARGET ANGLES
    // ---------------------------
    void GenerateTargets()
    {
        hourTargetAngle = Random.Range(-180f, 180f);
        minuteTargetAngle = Random.Range(-180f, 180f);
        Debug.Log("Hour Target: " + hourTargetAngle + ", Minute Target: " + minuteTargetAngle);
        UpdateDisplay();
    }

    // ---------------------------
    // MOUSE PLANE (RELATIVE METHOD)
    // ---------------------------
    Vector3 GetMousePointOnPlane()
    {
        Plane plane = new Plane(-cam.transform.forward, clockCenter.position);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return clockCenter.position;
    }

    // ---------------------------
    // DISPLAY
    // ---------------------------
    void UpdateDisplay()
    {
        if (resultText != null)
        {
            int currentHour = AngleToHour12(hourAngle);
            int targetHour = AngleToHour12(hourTargetAngle);
            int currentMinute = AngleToMinute(minuteAngle);
            int targetMinute = AngleToMinute(minuteTargetAngle);

            resultText.text = $"Time: {currentHour:D2}:{currentMinute:D2} / {targetHour:D2}:{targetMinute:D2}";
        }
    }

    void DisplayResult()
    {
        if (resultText != null)
        {
            int hour = AngleToHour12(hourAngle);
            int minute = AngleToMinute(minuteAngle);

            resultText.text = $"Success!\nTime: {hour:D2}:{minute:D2}";
        }
    }

    int AngleToHour12(float angle)
    {
        // Normalize angle to 0-360
        float normalizedAngle = angle % 360;
        if (normalizedAngle < 0) normalizedAngle += 360;

        // Convert angle to hour (1-12)
        int hour = Mathf.RoundToInt((normalizedAngle / 360f) * 12f) % 12;
        if (hour == 0) hour = 12;
        return hour;
    }

    int AngleToMinute(float angle)
    {
        // Normalize angle to 0-360
        float normalizedAngle = angle % 360;
        if (normalizedAngle < 0) normalizedAngle += 360;

        // Convert angle to minute (0-59)
        // Use floor instead of round for better accuracy
        int minute = Mathf.FloorToInt((normalizedAngle / 360f) * 60f) % 60;
        return minute;
    }
}