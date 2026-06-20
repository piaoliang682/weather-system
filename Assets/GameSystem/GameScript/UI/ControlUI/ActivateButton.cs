using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActivateButton : MonoBehaviour
{
    [Header("UI Objects To Activate")]
    public List<GameObject> objectsToActivate = new List<GameObject>();
    public Button activateButton;
    public Toggle toggleButton;
    private void Start()
    {
        if (activateButton != null)
            activateButton.onClick.AddListener(ToggleObjects);
        if (toggleButton != null)
            toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }


    private void OnToggleChanged(bool value)
    {
        ToggleObjects();
    }
    private void ToggleObjects()
    {
        foreach (var go in objectsToActivate)
        {
            if (go != null)
                go.SetActive(!go.activeSelf);
        }
    }

    private void ActivateObjects()
    {
        foreach (var go in objectsToActivate)
        {
            if (go != null)
                go.SetActive(true);
        }
    }

    private void DeactivateObjects()
    {
        foreach (var go in objectsToActivate)
        {
            if (go != null)
                go.SetActive(false);
        }
    }
}
