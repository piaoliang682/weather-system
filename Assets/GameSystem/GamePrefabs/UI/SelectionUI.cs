using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class PrefabScroller : MonoBehaviour
{
    [Header("References")]
    public Transform parentTransform;
    public PrefabVariantGroupSO prefabVariantGroupSO;

    [Header("Scroller Config")]
    public GameObject prefabCamera = null;
    public float minScale = 1f;
    public float maxScale = 1.5f;
    public float prefabSpace = 3f;
    public float moveForwardAmount = 2f;
    public float swipeThresholdX = 5f;
    public float rotateSpeed = 30f;
    public float snapTime = 0.3f;
    public float resetRotateSpeed = 180f;
    public ScrollerStyle scrollerStyle = ScrollerStyle.Line;
    [Range(0.1f, 1f)]
    public float scrollSpeedFactor = 0.25f;
    public Transform centerTransform;
    public Transform edgeTransform;
    public Transform startTransform;
    public Vector3 originalScale = Vector3.one;
    public Vector3 originalRotation = Vector3.zero;

    // state
    public string variantGroupName;
    public string variantName;

    // variant prefabs (from SO) and instantiated runtime objects
    private List<GameObject> variantPrefabs = new List<GameObject>();
    private List<GameObject> instantiatedPrefabs = new List<GameObject>();
    private List<Transform> instantiatedTransforms = new List<Transform>();

    private GameObject currentPrefabInstance;
    private GameObject lastCurrentCharacter;

    // single persistent rotation coroutine target
    private Transform rotatingTarget;

    // input
    private Vector3 startPos;
    private Vector3 endPos;
    private float startTime;
    private float endTime;
    private bool isCurrentCharacterRotating = false;
    private bool hasMoved = false;

    public enum ScrollerStyle
    {
        Line,
        Circle
    }

    // internals
    private float characterAngleSpace = 1f;
    private float currentAngle = 0f;
    private Vector3 cachedLocalCenter;

    private Coroutine snapCoroutine;

    private void OnEnable()
    {
        if (currentPrefabInstance)
        {
            StartRotateCurrentPrefab();
        }
    }

    void Start()
    {
        if (prefabVariantGroupSO == null)
        {
            Debug.LogWarning("PrefabScroller: prefabVariantGroupSO is not assigned.");
            return;
        }

        variantGroupName = prefabVariantGroupSO.varientGroupName;
        variantPrefabs = prefabVariantGroupSO.GetVariantList() ?? new List<GameObject>();

        if (variantPrefabs.Count == 0)
        {
            Debug.LogWarning("PrefabScroller: No variants found in the provided ScriptableObject.");
            return;
        }

        // ensure camera reference
        if (prefabCamera == null)
        {
            Debug.LogWarning("PrefabScroller: characterScrollerCamera not assigned.");
        }


        // determine starting index (variantName may be empty)
        int startingIndex = prefabVariantGroupSO.GetVariantIndexByName(variantName);
        startingIndex = Mathf.Clamp(startingIndex, 0, variantPrefabs.Count - 1);

        // convert center point to world once
        Vector3 centerPoint = centerTransform ? centerTransform.position : transform.position;

        // compute angle spacing used for circle layout
        characterAngleSpace = Mathf.PI * 2f / variantPrefabs.Count;
        currentAngle = startingIndex * characterAngleSpace;

        // spawn instances and cache transforms
        for (int i = 0; i < variantPrefabs.Count; i++)
        {
            GameObject prefab = variantPrefabs[i];
            if (prefab == null)
            {
                instantiatedPrefabs.Add(null);
                instantiatedTransforms.Add(null);
                continue;
            }

            GameObject inst = Instantiate(prefab, centerPoint, Quaternion.Euler(originalRotation));
            inst.transform.localScale = originalScale;
            inst.transform.SetParent(parentTransform, false);

            instantiatedPrefabs.Add(inst);
            instantiatedTransforms.Add(inst.transform);

            // configure initial layout
            switch (scrollerStyle)
            {
                case ScrollerStyle.Line:
                    if (prefabCamera) prefabCamera.GetComponent<Camera>().orthographic = true;
                    inst.transform.localPosition += new Vector3((i - startingIndex) * prefabSpace, 0f, 0f);
                    break;
                case ScrollerStyle.Circle:
                    if (prefabCamera) prefabCamera.GetComponent<Camera>().orthographic = false;
                    Vector3 localCenter = startTransform.InverseTransformPoint(centerPoint);
                    Vector3 localPoint = startTransform.InverseTransformPoint(edgeTransform.position);
                    float radius = Vector3.Distance(localCenter, localPoint);
                    inst.transform.localPosition = localCenter + new Vector3(Mathf.Sin(-currentAngle + i * characterAngleSpace), 0f, -Mathf.Cos(-currentAngle + i * characterAngleSpace)) * radius;
                    break;
            }
        }

        currentPrefabInstance = instantiatedPrefabs[startingIndex];
        if (currentPrefabInstance != null)
        {
            currentPrefabInstance.transform.localScale = maxScale * originalScale;
            if (scrollerStyle == ScrollerStyle.Line)
            {
                currentPrefabInstance.transform.localPosition += moveForwardAmount * Vector3.forward;
            }
        }

        lastCurrentCharacter = null;

        // start single persistent rotation coroutine
        StartCoroutine(RotationLoop());
        if (currentPrefabInstance != null) StartRotateCurrentPrefab();
    }

    void Update()
    {
        // cache local center once per frame
        cachedLocalCenter = startTransform.InverseTransformPoint(centerTransform.position);

        HandleInput();
    }

    #region Input Handling

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) HandleTouchStart();
        else if (Input.GetMouseButton(0)) HandleTouchDrag();
        else if (Input.GetMouseButtonUp(0)) HandleTouchRelease();
    }

    private void HandleTouchStart()
    {
        startPos = Input.mousePosition;
        startTime = Time.time;
        hasMoved = false;
    }

    private void HandleTouchDrag()
    {
        endPos = Input.mousePosition;
        endTime = Time.time;

        float deltaX = Mathf.Abs(startPos.x - endPos.x);
        if (deltaX >= swipeThresholdX)
        {
            hasMoved = true;

            // stop rotating and reset rotation smoothly
            if (isCurrentCharacterRotating)
                StopRotateCurrentPrefab(true);

            float speed = deltaX / Mathf.Max(0.0001f, (endTime - startTime));
            Vector3 dir = (startPos.x - endPos.x < 0f) ? Vector3.right : Vector3.left;
            Vector3 moveVector = dir * (speed / 10f) * scrollSpeedFactor * Time.deltaTime;

            // update angle for circle mode
            currentAngle -= moveVector.x * characterAngleSpace / 5f;
            currentAngle = Mathf.Repeat(currentAngle, Mathf.PI * 2f);

            // move and scale instances
            int count = instantiatedTransforms.Count;
            for (int i = 0; i < count; i++)
            {
                Transform tf = instantiatedTransforms[i];
                if (tf == null) continue;

                switch (scrollerStyle)
                {
                    case ScrollerStyle.Line:
                        MoveAndScale(tf, moveVector, cachedLocalCenter);
                        break;
                    case ScrollerStyle.Circle:
                        MoveAndScaleCircleVer(tf, moveVector, i, cachedLocalCenter);
                        break;
                }
            }

            startPos = endPos;
            startTime = endTime;
        }
    }

    private void HandleTouchRelease()
    {
        if (!hasMoved) return;

        lastCurrentCharacter = currentPrefabInstance;

        switch (scrollerStyle)
        {
            case ScrollerStyle.Line:
                currentPrefabInstance = FindPrefabNearestToCenterLine();
                if (currentPrefabInstance != null)
                {
                    float snapDistance = cachedLocalCenter.x - currentPrefabInstance.transform.localPosition.x;
                    if (snapCoroutine != null) StopCoroutine(snapCoroutine);
                    snapCoroutine = StartCoroutine(SnapAndRotate(snapDistance));
                }
                break;
            case ScrollerStyle.Circle:
                currentPrefabInstance = FindPrefabNearestToCenterCircleVer();
                if (snapCoroutine != null) StopCoroutine(snapCoroutine);
                snapCoroutine = StartCoroutine(SnapAndRotateCircleVer());
                break;
        }
    }

    #endregion

    #region Movement & Scaling

    private void MoveAndScale(Transform tf, Vector3 moveVector, Vector3 localCenter)
    {
        // Move
        tf.localPosition += moveVector;

        // Scale and move forward according to distance from current position to center position
        float d = Mathf.Abs(tf.localPosition.x - localCenter.x);
        if (d < (prefabSpace / 2f))
        {
            float factor = 1f - d / (prefabSpace / 2f);
            float scaleFactor = Mathf.Lerp(minScale, maxScale, factor);
            tf.localScale = scaleFactor * originalScale;

            float fd = Mathf.Lerp(0f, moveForwardAmount, factor);
            Vector3 pos = tf.localPosition;
            pos.z = localCenter.z + fd;
            tf.localPosition = pos;
        }
        else
        {
            tf.localScale = minScale * originalScale;
            Vector3 pos = tf.localPosition;
            pos.z = localCenter.z;
            tf.localPosition = pos;
        }
    }

    private void MoveAndScaleCircleVer(Transform tf, Vector3 moveVector, int index, Vector3 localCenter)
    {
        // Move by recalculating polar position from angle
        Vector3 localPoint = startTransform.InverseTransformPoint(edgeTransform.position);
        float radius = Vector3.Distance(localCenter, localPoint);
        tf.localPosition = localCenter + new Vector3(Mathf.Sin(-currentAngle + index * characterAngleSpace), 0f, -Mathf.Cos(-currentAngle + index * characterAngleSpace)) * radius;

        // Determine angular distance to center
        float d = Mathf.Abs(Vector3.Angle(Vector3.back, (tf.localPosition - localCenter).normalized) * Mathf.Deg2Rad);
        if (d < (characterAngleSpace / 2f))
        {
            float factor = 1f - d / (characterAngleSpace / 2f);
            float scaleFactor = Mathf.Lerp(minScale, maxScale, factor);
            tf.localScale = scaleFactor * originalScale;
        }
        else
        {
            tf.localScale = minScale * originalScale;
        }
    }

    #endregion

    #region Find Nearest

    private GameObject FindPrefabNearestToCenterLine()
    {
        float minDistance = float.MaxValue;
        GameObject nearest = null;
        for (int i = 0; i < instantiatedTransforms.Count; i++)
        {
            Transform tf = instantiatedTransforms[i];
            if (tf == null) continue;
            float distance = Mathf.Abs(tf.localPosition.x - cachedLocalCenter.x);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = tf.gameObject;
            }
        }
        return nearest;
    }

    private GameObject FindPrefabNearestToCenterCircleVer()
    {
        int nearestIndex = Mathf.RoundToInt(currentAngle / characterAngleSpace) % instantiatedTransforms.Count;
        if (nearestIndex < 0) nearestIndex += instantiatedTransforms.Count;
        return instantiatedPrefabs[nearestIndex];
    }

    #endregion

    #region Snap Coroutines

    private IEnumerator SnapAndRotate(float snapDistance)
    {
        float snapDistanceAbs = Mathf.Abs(snapDistance);
        if (snapDistanceAbs < 0.0001f) yield break;

        float snapSpeed = snapDistanceAbs / snapTime;
        float sign = Mathf.Sign(snapDistance);
        float movedDistance = 0f;

        while (Mathf.Abs(movedDistance) < snapDistanceAbs)
        {
            float d = sign * snapSpeed * Time.deltaTime;
            float remainedDistance = Mathf.Abs(snapDistanceAbs - Mathf.Abs(movedDistance));
            d = Mathf.Clamp(d, -remainedDistance, remainedDistance);

            Vector3 moveVector = new Vector3(d, 0f, 0f);
            for (int i = 0; i < instantiatedTransforms.Count; i++)
            {
                Transform tf = instantiatedTransforms[i];
                if (tf == null) continue;
                MoveAndScale(tf, moveVector, cachedLocalCenter);
            }

            movedDistance += d;
            yield return null;
        }

        // ensure rotating state
        if (currentPrefabInstance != lastCurrentCharacter || !isCurrentCharacterRotating)
        {
            StopRotateCurrentPrefab();
            StartRotateCurrentPrefab();
        }
    }

    private IEnumerator SnapAndRotateCircleVer()
    {
        float nextAngle = Mathf.Round(currentAngle / characterAngleSpace) * characterAngleSpace;
        while (Mathf.Abs(currentAngle - nextAngle) > 0.01f)
        {
            Vector3 moveVector = new Vector3((nextAngle - currentAngle) / snapTime * 10f * Time.deltaTime, 0f, 0f);
            currentAngle += moveVector.x * characterAngleSpace;
            currentAngle = Mathf.Repeat(currentAngle, Mathf.PI * 2f);

            for (int i = 0; i < instantiatedTransforms.Count; i++)
            {
                Transform tf = instantiatedTransforms[i];
                if (tf == null) continue;
                MoveAndScaleCircleVer(tf, moveVector, i, cachedLocalCenter);
            }

            yield return null;
        }

        if (currentPrefabInstance != lastCurrentCharacter || !isCurrentCharacterRotating)
        {
            StopRotateCurrentPrefab();
            StartRotateCurrentPrefab();
        }
    }

    #endregion

    #region Rotation Handling

    private IEnumerator RotationLoop()
    {
        while (true)
        {
            if (rotatingTarget != null)
            {
                rotatingTarget.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f), Space.Self);
            }
            yield return null;
        }
    }

    void StartRotateCurrentPrefab()
    {
        if (currentPrefabInstance == null) return;
        rotatingTarget = currentPrefabInstance.transform;
        isCurrentCharacterRotating = true;
    }

    void StopRotateCurrentPrefab(bool resetRotation = false)
    {
        isCurrentCharacterRotating = false;
        rotatingTarget = null;

        if (resetRotation && currentPrefabInstance != null)
        {
            StartCoroutine(ResetPrefabRotation(currentPrefabInstance.transform));
        }
    }

    private IEnumerator ResetPrefabRotation(Transform charTf)
    {
        if (charTf == null) yield break;

        Vector3 startRotation = charTf.rotation.eulerAngles;
        Vector3 endRotation = originalRotation;
        float timePast = 0f;
        float rotateAngle = Mathf.Abs(Mathf.DeltaAngle(startRotation.y, endRotation.y));
        float rotateTime = rotateAngle / resetRotateSpeed;
        if (rotateTime <= 0f) yield break;

        while (timePast < rotateTime)
        {
            timePast += Time.deltaTime;
            Vector3 rotation = Vector3.Lerp(startRotation, endRotation, timePast / rotateTime);
            charTf.rotation = Quaternion.Euler(rotation);
            yield return null;
        }

        charTf.rotation = Quaternion.Euler(endRotation);
    }

    #endregion

    #region Public getters

    public string GetCurrentVariantGroupName()
    {
        return variantGroupName;
    }

    public string GetCurrentVariantName()
    {
        return variantName;
    }

    public int GetCurrentVariantIndex()
    {
        if (currentPrefabInstance == null || prefabVariantGroupSO == null) return -1;
        return prefabVariantGroupSO.GetVariantIndexByName(currentPrefabInstance.name);
    }

    #endregion
}
