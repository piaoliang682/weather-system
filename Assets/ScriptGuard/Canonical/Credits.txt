using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    public GameObject creditsPanel;
    public Text nameText;
    public Text classText;
    public Text idText;

    public string nameString;
    public string classString;
    public string idString;

    public Button toggleButton;
    public KeyCode toggleKey = KeyCode.F1;

    private bool panelVisible = false;

    void Start()
    {
        creditsPanel.SetActive(panelVisible);

        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleCredits);

        // Assign the UI text fields
        nameText.text = nameString;
        classText.text = classString;
        idText.text = idString;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            ToggleCredits();
    }

    void ToggleCredits()
    {
        panelVisible = !panelVisible;
        creditsPanel.SetActive(panelVisible);
    }
}
