using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessagePopUpPanel : InteractivePanelBase
{
    public TMP_Text titleText;
    public TMP_Text messageText;

    // Start is called before the first frame update

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
    public void SetTitle(string message)
    {
        titleText.text = message;
    }
}

