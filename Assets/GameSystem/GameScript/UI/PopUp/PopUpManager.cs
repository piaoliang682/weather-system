using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }

    [Header("References")]
    public Transform popupContainer;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public GameObject SpawnPopup( GameObject popupPrefab)
    {
        if (popupContainer == null || popupPrefab == null) return null;
        GameObject popupInstance = Instantiate(popupPrefab, popupContainer);
        RectTransform rect = popupInstance.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
        }
        Debug.Log(popupInstance.name);
        return popupInstance;
    }
    private void OnDestroy()
    {
        Debug.Log("this should not destroyed");
    }
}
