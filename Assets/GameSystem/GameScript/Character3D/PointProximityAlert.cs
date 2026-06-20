using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Transform))]
public class MeshProximityAlert : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public TMP_Text alertText;
    public TMP_Text pathText;
    [Header("Detection")]
    public float alertDistance = 2f;
    public float checkInterval = 0.1f;
    public LayerMask meshLayerMask;
    public string targetTag = "Path";

    [Header("Ray Settings")]
    public int raysPerRing = 16;
    public int verticalRings = 3;
    public bool useSphereCast = false;
    public float sphereRadius = 0.2f;

    private float timeSinceLastCheck;
    private Vector3 lastPlayerPosition;
    private bool isOnPath;

    void Start()
    {
        isOnPath = false;
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;
            CheckMeshProximity();
            if (!isOnPath)
            {
                PlayAlert("NotOnPath");
            }
        }
    }

    void CheckMeshProximity()
    {
        ClearAlert();

        Vector3 origin = player.position;
        Vector3 movementDir = (origin - lastPlayerPosition).normalized;
        lastPlayerPosition = origin;

        bool detectedPathThisFrame = false;
        Vector3 closestPathPoint = Vector3.zero;

        // Downward
        TryCast(origin, Vector3.down, ref detectedPathThisFrame, ref closestPathPoint);

        // Horizontal
        for (int i = 0; i < raysPerRing; i++)
        {
            float angle = i * Mathf.PI * 2f / raysPerRing;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            TryCast(origin, dir, ref detectedPathThisFrame, ref closestPathPoint);
        }

        // Vertical rings
        for (int vr = 1; vr <= verticalRings; vr++)
        {
            float pitch = (vr / (float)(verticalRings + 1)) * Mathf.PI;
            for (int i = 0; i < raysPerRing; i++)
            {
                float angle = i * Mathf.PI * 2f / raysPerRing;
                Vector3 dir = new Vector3(
                    Mathf.Sin(pitch) * Mathf.Cos(angle),
                    Mathf.Cos(pitch),
                    Mathf.Sin(pitch) * Mathf.Sin(angle)
                );
                TryCast(origin, dir, ref detectedPathThisFrame, ref closestPathPoint);
            }
        }
    }

    void TryCast(Vector3 origin, Vector3 direction, ref bool detected, ref Vector3 hitPoint)
    {
        RaycastHit hit;
        bool hasHit = useSphereCast
            ? Physics.SphereCast(origin, sphereRadius, direction, out hit, alertDistance, meshLayerMask)
            : Physics.Raycast(origin, direction, out hit, alertDistance, meshLayerMask);

        if (!hasHit)
        {
            Debug.DrawRay(origin, direction * alertDistance, Color.green, checkInterval);
            return;
        }

        Debug.DrawLine(origin, hit.point, Color.red, checkInterval);


            detected = true;
            hitPoint = hit.point;
            AlertPlayer(hit.collider.tag, hit.point);
    }

    void AlertPlayer(string tag, Vector3 hitPoint)
    {

        if (tag!="Path")
        {
            Debug.Log($"⚠️ ALERT: Near {tag}");
            alertText.text = $"Near {tag}";
            PlayAlert("Alert"+tag);
        }
        else
        {
            isOnPath = true;
            pathText.text = $"On {tag}";
        }

    }

    void PlayAlert(string tg)
    {
        if (GameReference.UIAudioSource == null)
            return;

        //Handheld.Vibrate();
        // If already playing, do NOT restart
        if (GameReference.UIAudioSource.isPlaying)
            return;

        var def = GameReference.AudioRegistry.GetDefinition(tg);
        if (def == null || def.clip == null)
            return;

        GameReference.UIAudioSource.clip = def.clip;
        GameReference.UIAudioSource.Play();

    }

    void ClearAlert()
    {
        alertText.text = "Safe to walk";
        pathText.text = "Not on Path";
        isOnPath = false;
    }


}
