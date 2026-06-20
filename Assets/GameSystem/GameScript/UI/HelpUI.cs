using UnityEngine;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject helpPanel;
    public Text titleText;
    public Text descriptionText;
    public Button helpButton;
    public Button closeButton;

    [Header("Help Content")]
    public string helpTitle = "Help";
    [TextArea(3, 10)]
    public string helpDescription = "This is your help text. Provide instructions here.";

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.F1;
    public bool openOnStart = false;  // NEW: set panel visibility at start

    private bool isVisible = false;
    void Start()
    {
        isVisible = openOnStart;
        // Initialize UI content
        titleText.text = helpTitle;
        if (descriptionText!=null)
            descriptionText.text = helpDescription;

        // Set initial visibility from openOnStart
        helpPanel.SetActive(openOnStart);

        // Hook up buttons if assigned
        if (helpButton != null)
            helpButton.onClick.AddListener(ToggleHelp);
        if (closeButton != null)
            closeButton.onClick.AddListener(ToggleHelp);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            ToggleHelp();
    }

    private void ToggleHelp()
    {
        Debug.Log("toggle");
         isVisible = !isVisible;
        helpPanel.SetActive(isVisible);
    }
}
