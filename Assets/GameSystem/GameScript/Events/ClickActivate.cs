using UnityEngine;
using UnityEngine.Events;



public class ClickActivate : MonoBehaviour
{
    [Header("Targets to Affect")]
    public GameObject[] targets;

    [Header("Action to Perform")]
    public ActivationAction action;

    [Header("Optional Unity Event")]
    public UnityEvent onClick; // Trigger extra events if needed

    // This method should be linked to a UI Button or called on mouse click
    public void ActivateTargets()
    {
        foreach (GameObject obj in targets)
        {
            if (obj == null) continue;

            switch (action)
            {
                case ActivationAction.Enable:
                    obj.SetActive(true);
                    break;

                case ActivationAction.Disable:
                    obj.SetActive(false);
                    break;

                case ActivationAction.Instantiate:
                    Instantiate(obj, obj.transform.position, obj.transform.rotation);
                    break;

                case ActivationAction.Destroy:
                    Destroy(obj);
                    break;
            }
        }
    }

    // Example: if this script is on a clickable object in the scene
    private void OnMouseDown()
    {
        ActivateTargets();
        Debug.Log("cl;ick on "+gameObject.name);
        onClick?.Invoke();
    }
}
public enum ActivationAction
{
    Enable,
    Disable,
    Instantiate,
    Destroy
}