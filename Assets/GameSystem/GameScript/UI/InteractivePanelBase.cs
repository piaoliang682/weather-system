using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
public abstract class InteractivePanelBase : MonoBehaviour
{
    [Header("UI References")]
    public bool showOnStart=false;
    public GameObject panel;

    public List<Button> triggerButtons = new List<Button>();
    public Button closeButton;

    //public Text messageText;          // Main message
    public Text feedbackText;         // Temporary feedback for failed actions

    [HideInInspector]
    public AudioSource audioSource;

    public UnityEvent onTrigger; // visible in the Inspector

    //[TextArea(10, 20)]
    //[Header("Interaction Settings")]
    //public string message = @"Interact with this object";
    public float feedbackClearDelay = 2f;

    protected bool playerInRange = false;
    private Coroutine feedbackCoroutine;

    protected virtual void Awake()
    {
        if (triggerButtons != null)
        {
            foreach (Button btn in triggerButtons)
            {
                btn.onClick.AddListener(TogglePanel);
                btn.onClick.AddListener(PlayButtonClickSound);
            }

        }
        audioSource = GetComponent<AudioSource>();
        if (panel != null)
            panel.SetActive(showOnStart);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
            closeButton.onClick.AddListener(PlayButtonClickSound);
        }




    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowPanel();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ClosePanel();
        }
    }

    protected virtual void ShowPanel()
    {
        EffectOnShow();
        if (panel != null)
        {
            panel.SetActive(true);

            //if (messageText != null) messageText.text = message;
            ClearFeedback();
        }
    }

    protected virtual void ClosePanel()
    {
        if (panel != null)
            panel.SetActive(false);

        ClearFeedback();
    }

    protected virtual void TogglePanel()
    {

        if (panel != null)
        {

            if (panel.activeSelf == false)
            {
                ShowPanel();
                ClearFeedback();
            }
            else {
                ClosePanel();
                EffectOnShow();
            }
        }
    }

    private void EffectOnShow()
    {
        onTrigger.Invoke();
        if (audioSource != null)
        {
            //triggerAudioSource.PlayOneShot(G);
        }
    }
    protected virtual void PlayButtonClickSound()
    {

            var def = GameReference.AudioRegistry.GetDefinition("ButtonClick");

        GameReference.UIAudioSource.PlayOneShot(def.clip, def.volume);
    }
    /// <summary>
    /// Optional override to customize the failure message.
    /// </summary>
    protected virtual string GetFailureMessage()
    {
        return "Action failed.";
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
            feedbackText.text = "";
    }

    public Button GetCloseButton()
    {
        return closeButton;
    }
}
