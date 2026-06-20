using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;       // The menu panel to show/hide
    [SerializeField] private Button gearButton;       // Optional: Assign a UI Button for the gear
    [SerializeField] private Button continueButton;       // Optional: Assign a UI Button for the gear
    public bool isOpenOnStart;
    private bool isMenuOpen = false;

    void Start()
    {
        // Optionally assign the button's onClick event if it's provided
        if (gearButton != null)
        {
            gearButton.onClick.AddListener(ToggleMenu);
            continueButton.onClick.AddListener(ToggleMenu);
        }

        // Make sure menu is hidden at start
        menuUI.SetActive(isOpenOnStart);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuUI.SetActive(isMenuOpen);
            Debug.Log(isMenuOpen);
        // Pause/unpause game
       // Time.timeScale = isMenuOpen ? 0f : 1f;

    }
}
