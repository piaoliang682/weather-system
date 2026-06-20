using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    [Header("Target Door")]
    private Transform door; // assign the actual door mesh here

    [Header("Door Settings")]
    public bool isEnabled = true;

    public float openAngle = 90f;
    public float duration = 0.5f;
    public Ease easeType = Ease.OutQuad;

    [Header("Current State")]
    public bool isOpen = false;

    private Tween currentTween;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {


            door=gameObject.transform;



        closedRotation = door.localRotation;

        openRotation = Quaternion.Euler(
            door.localEulerAngles.x,
            door.localEulerAngles.y + openAngle,
            door.localEulerAngles.z
        );
    }

    public void ToggleDoor()
    {
        if (!isEnabled) return;

        Debug.Log(isOpen  + " door: " + gameObject.name);
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        //if currently open, rotate to closed; if closed, rotate to open
        currentTween = door.DOLocalRotate(
            isOpen ? closedRotation.eulerAngles : openRotation.eulerAngles,
            duration
        ).SetEase(easeType);
        isOpen = !isOpen;


    }

    //public void OpenDoor()
    //{
    //    if (!isEnabled || isOpen) return;
    //    ToggleDoor();
    //}

    //public void CloseDoor()
    //{
    //    if (!isEnabled || !isOpen) return;
    //    ToggleDoor();
    //}

    //public void SetEnabled(bool value)
    //{
    //    isEnabled = value;
    //}
}