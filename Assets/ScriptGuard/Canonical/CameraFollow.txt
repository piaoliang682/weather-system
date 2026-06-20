using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Targets")]
    public List<Transform> targets = new List<Transform>();
    private int currentIndex = 0;

    [Header("Follow Settings")]
    public Vector3 positionOffset = new Vector3(0, 0, -10);
    public float followSpeed = 10f;

    [Header("Rotation")]
    public bool followRotation = false;
    public float rotationSpeed = 10f;

    private Transform currentTarget;
    private Coroutine snapRoutine;
    private void Start()
    {
        if (targets.Count > 0)
            SetTargetByIndex(0);
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
            return;

        Vector3 desiredPos = currentTarget.position + positionOffset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            Time.deltaTime * followSpeed
        );

        if (followRotation)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                currentTarget.rotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    // ---------- PUBLIC API ----------

    public void NextTarget()
    {
        if (targets.Count == 0) return;

        currentIndex = (currentIndex + 1) % targets.Count;
        SetTargetByIndex(currentIndex);
    }

    public void PreviousTarget()
    {
        if (targets.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = targets.Count - 1;

        SetTargetByIndex(currentIndex);
    }

    public void SetTargetByIndex(int index)
    {
        if (index < 0 || index >= targets.Count)
            return;

        currentIndex = index;
        currentTarget = targets[index];

        SnapToTarget();
    }

    public void AddTarget(Transform target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    public void RemoveTarget(Transform target)
    {
        if (targets.Remove(target))
        {
            if (currentIndex >= targets.Count)
                currentIndex = 0;

            SetTargetByIndex(currentIndex);
        }
    }

    private void SnapToTarget()
    {
        if (currentTarget == null)
            return;

        if (snapRoutine != null)
            StopCoroutine(snapRoutine);

        snapRoutine = StartCoroutine(SmoothSnap());
    }

    private IEnumerator SmoothSnap()
    {
        float duration = 0.5f; // time to reach target (seconds)
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = currentTarget.position + positionOffset;
        Quaternion targetRot = followRotation ? currentTarget.rotation : transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
    }
}
