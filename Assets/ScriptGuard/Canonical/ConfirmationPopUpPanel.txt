using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ConfirmationPopUpPanel : InteractivePanelBase
{
    public TMP_Text titleText;
    public TMP_Text messageText;
    public Image icon;

    public Button confirmButton;
    public Button rejectButton;
    // Start is called before the first frame update
    void Start()
    {
        confirmButton.onClick.AddListener(ClosePanel);
        confirmButton.onClick.AddListener(PlayButtonClickSound);
        rejectButton.onClick.AddListener(ClosePanel);
        rejectButton.onClick.AddListener(PlayErrorSound);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CanClose(bool b)
    {
        foreach(var btn in triggerButtons)
        {
            btn.gameObject.SetActive(b);
        }
    }
    public Button GetRejectButton()
    {
        return rejectButton;
    }
    public Button GetConfirmButton()
    {
        return confirmButton;
    }

    public void SetHideAfterClick()
    {
        confirmButton.onClick.AddListener(PlayButtonClickSound);
        confirmButton.onClick.AddListener(ClosePanel);
        rejectButton.onClick.AddListener(PlayErrorSound);
        rejectButton.onClick.AddListener(ClosePanel);
    }
    public void SetMessage(string message)
    {
        messageText.text = message;
    }
    public void SetTitle(string message)
    {
        titleText.text = message;
    }
    public void SetIcon(Sprite img)
    {
        if (img == null)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        icon.gameObject.SetActive(true);
        icon.sprite = img;
    }
    private void PlayErrorSound()
    {
        var def = GameReference.AudioRegistry.GetDefinition("Error");

           


        GameReference.UIAudioSource.PlayOneShot(def.clip, def.volume);
        Debug.Log("audioplayed");
    }
}
