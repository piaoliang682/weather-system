using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class InputToTextDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public TMP_Text outputText;
    public UnityEvent onTextSubmit;
    [Header("Settings")]
    public bool appendMode = true;

    void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        inputField.onSubmit.AddListener(HandleSubmit);
    }

    private void HandleSubmit(string text)
    {
        onTextSubmit?.Invoke(); 
        if (string.IsNullOrWhiteSpace(text))
            return;

        if (appendMode)
        {
            outputText.text += text + "\n";
        }
        else
        {
            outputText.text = text;
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

    void OnDestroy()
    {
        inputField.onSubmit.RemoveListener(HandleSubmit);
    }
}