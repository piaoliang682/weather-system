using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTeleportNode : MonoBehaviour
{
    [Header("Popup Settings")]
    [SerializeField] private PopUpDefinition popUpSO;
    [SerializeField] private string sceneName;
    [SerializeField] private Button sceneButton;

    [SerializeField] private bool isSaveAsSpawnPoint;
    private GameObject confirmationPopup;

    private void Start()
    {
        if(sceneButton!=null)
        sceneButton.onClick.AddListener(EnterScene);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!other.CompareTag("Player")) return;
        EnterScene();

    }

    private void EnterScene()
    {
        if (PopUpManager.Instance == null)
        {
            SceneManager.LoadScene(sceneName);
            Debug.LogWarning("PopupManager not found in scene.");
            return;
        }

        // Spawn confirmation popup
        confirmationPopup = PopUpManager.Instance.SpawnPopup(GameReference.ConfirmationPopUp);
        if (confirmationPopup == null) return;

        ConfirmationPopUpPanel ui = confirmationPopup.GetComponent<ConfirmationPopUpPanel>();
        if (ui == null) return;

        ui.SetTitle(popUpSO.PromptTitle);
        ui.SetMessage(popUpSO.PromptText);
        ui.SetIcon(null); // Optional: set icon if needed

        // Wire buttons
        ui.GetConfirmButton().onClick.AddListener(Confirm);

        ui.GetRejectButton().onClick.AddListener(Reject);
    }

    private void Confirm()
    {
            // Destroy confirmation popup
            Destroy(confirmationPopup);

            // Load the target scene if specified
            if (!string.IsNullOrEmpty(sceneName))
            {
            if (isSaveAsSpawnPoint)
            {
                SpawnPointManager.Instance.SetCurrentSpawnPoint( gameObject.transform);
            }
                SceneManager.LoadScene(sceneName);
            }
        
    }

    private void Reject()
    {
        Destroy(confirmationPopup);
    }
}
