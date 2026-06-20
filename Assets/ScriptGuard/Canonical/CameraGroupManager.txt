using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraGroupManager : MonoBehaviour
{
    public static CameraGroupManager Instance;

    [Header("Default Camera")]
    public Camera defaultCamera;

    private Camera activeCamera;
    private int currentCameraIndex = 0;
    public List <Camera> cameras = new List<Camera>();  
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetActiveCamera(defaultCamera);
    }

    // =========================
    // SWITCH CAMERA
    // =========================
    public void SetActiveCamera(Camera cam)
    {
        if (cam == null) return;

        // disable previous camera
        if (activeCamera != null)
            activeCamera.enabled = false;


        foreach (Camera c in cameras)
        {
            if (c != activeCamera)
                c.enabled = false;
        }
        // enable new camera
        activeCamera = cam;
        activeCamera.enabled = true;

    }

    public void SetCameraByIndex(int index)
    {
        if (index < 0 || index >= cameras.Count) return;
        SetActiveCamera(cameras[index]);
        currentCameraIndex = index;
    }
    public void NextCamera()
    {
        SetActiveCamera(cameras[currentCameraIndex+=1]);
    }

    public void PreviousCamera()
    {
        SetActiveCamera(cameras[currentCameraIndex -= 1]);
    }
    // =========================
    // RETURN TO DEFAULT
    // =========================
    public void ResetToDefault()
    {
        SetActiveCamera(defaultCamera);
    }

    // =========================
    // GET ACTIVE CAMERA
    // =========================
    public Camera GetActiveCamera()
    {
        return activeCamera;
    }
}