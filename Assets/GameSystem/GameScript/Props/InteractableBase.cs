using UnityEngine;

public class InteractableBase : MonoBehaviour,IInteractable
{
    [Header("Interaction Settings")]
    [Tooltip("Message shown to player when near this object.")]
    protected string promptTitle= "Stole this item?";
    [TextArea]
    protected string promptText = "stealinng is a crime, but I am poor";
    [Tooltip("Should do choice and cannot ignore")]
    public bool isImmediate = true;

    public bool needInput = true;
    [Tooltip("Show a confirmation popup before handling interaction?")]
    public bool askConfirmation = true;
    [Tooltip("can come back later to choose, but only choose once")]
    public bool oneOff=true;
    [TextArea]
    protected string confirmFeed;
    [TextArea]
    protected string rejectFeed;

    protected float lifeTime=3f;

    protected Sprite icon;

    [SerializeField] private PopUpDefinition PopUpDifinition;

    private GameObject character;
    private Interact characterInteractScript;
    /// <summary>
    /// Called when player presses the interact key
    /// </summary>
    /// 
    protected virtual void Start()
    {

        promptTitle = PopUpDifinition.PromptTitle;
        promptText = PopUpDifinition.PromptText;
        confirmFeed = PopUpDifinition.ConfirmFeed;
        rejectFeed = PopUpDifinition.RejectFeed;
        icon = PopUpDifinition.Icon;
    }
    public virtual void HandleInteraction(GameObject interactor)
    {
        SetInteractor(interactor);
        if (askConfirmation)
            SpawnConfirmationPopup();
        else
        {
            OnConfirmed();
        }


    }
    protected virtual void OnTriggerEnter(Collider other)
    {

            characterInteractScript = other.GetComponent<Interact>();
        if(characterInteractScript!=null)
            characterInteractScript.SetCurrentInteractable(this);
        else
        {
            return;
        }

        if (!needInput)
        {
                HandleInteraction(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (characterInteractScript == other.GetComponent<Interact>())
        {
            characterInteractScript.SetCurrentInteractable(null);
            characterInteractScript = null;

        }

    }
    public virtual void SetInteractor(GameObject interactor)
    {

        character = interactor;
    }

    public virtual bool GetNeedInput()
    {
        return needInput;
    }

    /// <summary>
    /// Show tips when player approaches
    /// </summary>


    #region PopupManager Integration

    protected void SpawnResultPopup(string message)
    {
        if (PopUpManager.Instance == null || GameReference.PopUp == null) return;

        GameObject popup = PopUpManager.Instance.SpawnPopup(GameReference.PopUp);
        if (popup == null) return;

        PopUp ui = popup.GetComponent<PopUp>();
        if (ui == null) return;
        ui.SetMessage(message);
        ui.SetLifeTime(lifeTime);
    }

    protected void SpawnConfirmationPopup()
    {
        if (PopUpManager.Instance == null || GameReference.ConfirmationPopUp == null) return;

        GameObject popup = PopUpManager.Instance.SpawnPopup(GameReference.ConfirmationPopUp);
        if (popup == null) return;

        ConfirmationPopUpPanel ui = popup.GetComponent<ConfirmationPopUpPanel>();
        if (ui == null) return;

        ui.SetTitle(promptTitle);
        ui.SetMessage(promptText);
        ui.SetIcon(null);
        if (isImmediate && ui.GetCloseButton() != null)
        {
            ui.GetCloseButton().gameObject.SetActive(false);
        }

        ui.GetConfirmButton().onClick.AddListener(OnConfirmed);

        ui.GetRejectButton().onClick.AddListener( OnReject);
    }

    /// <summary>
    /// Override this in subclasses for custom logic after confirmation
    /// </summary>
    protected virtual void OnConfirmed()
    {
        SpawnResultPopup(confirmFeed);
        Debug.Log($"Confirmed interaction with {gameObject.name}");
        if (oneOff)
        {
            gameObject.SetActive(false);
        }
    }
    protected virtual void OnReject()
    {
        SpawnResultPopup(rejectFeed);
        Debug.Log($"reject interaction with {gameObject.name}");
        if (oneOff||isImmediate)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}
