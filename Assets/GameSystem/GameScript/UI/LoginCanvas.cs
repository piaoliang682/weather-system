using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginCanvas : MonoBehaviour
{
    [Header("Scene")]
    public string scene1;

    [Header("Login Panel")]
    public GameObject loginPanel;
    public InputField accountIF;
    public InputField passwordIF;
    public Button loginBtn;
    public Button registerPanelBtn;
    public Button changePasswordPanelBtn;

    [Header("Register Panel")]
    public GameObject registerPanel;
    public InputField registerAccountIF;
    public InputField registerPasswordIF;
    public Button registerBtn;
    public Button backBtn;

    [Header("Change Password Panel")]
    public GameObject changePasswordPanel;
    public InputField changeAccountIF;
    public InputField changePasswordIF;
    public Button changePasswordSureBtn;
    public Button changePasswordBackBtn;

    [Header("Toast")]
    public GameObject toast;
    public Text toastContent;

    public static LoginCanvas Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RegisterButtonListeners();
    }

    #region Button Registration

    private void RegisterButtonListeners()
    {
        loginBtn.onClick.AddListener(OnLoginButtonClicked);
        registerPanelBtn.onClick.AddListener(() => ShowPanel(false, true, false));
        changePasswordPanelBtn.onClick.AddListener(() => ShowPanel(false, false, true));
        backBtn.onClick.AddListener(() => ShowPanel(true, false, false));
        changePasswordBackBtn.onClick.AddListener(() => ShowPanel(true, false, false));
        registerBtn.onClick.AddListener(OnRegisterButtonClicked);
        changePasswordSureBtn.onClick.AddListener(OnChangePasswordButtonClicked);
    }

    #endregion

    #region Login

    private void OnLoginButtonClicked()
    {
        string account = accountIF.text;
        string password = passwordIF.text;

        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
        {
            ShowToast("’À∫≈√‹¬ÎŒ™ø’");
            return;
        }

        if (!PlayerPrefs.HasKey(account))
        {
            ShowToast("’À∫≈≤ª¥Ê‘⁄");
            return;
        }

        if (PlayerPrefs.GetString(account) == password)
        {
            PlayerPrefs.SetString(GlobalValue.CURRENT_LOGIN_ACCOUNT_KEY, account);
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene1);
        }
        else
        {
            ShowToast("’À∫≈ªÚ√‹¬Î¥ÌŒÛ");
        }
    }

    #endregion

    #region Register

    private void OnRegisterButtonClicked()
    {
        string account = registerAccountIF.text;
        string password = registerPasswordIF.text;

        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
        {
            ShowToast("’À∫≈√‹¬ÎŒ™ø’");
            return;
        }

        if (PlayerPrefs.HasKey(account))
        {
            ShowToast($"{account}’À∫≈“—±ª◊¢≤·£¨«Î◊¢≤·∆‰À˚");
            return;
        }

        PlayerPrefs.SetString(account, password);
        ShowToast("◊¢≤·≥…π¶");
        ShowPanel(true, false, false);
    }

    #endregion

    #region Change Password

    private void OnChangePasswordButtonClicked()
    {
        string account = changeAccountIF.text;
        string password = changePasswordIF.text;

        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
        {
            ShowToast("’À∫≈√‹¬ÎŒ™ø’");
            return;
        }

        if (!PlayerPrefs.HasKey(account))
        {
            ShowToast("’À∫≈≤ª¥Ê‘⁄");
            return;
        }

        PlayerPrefs.SetString(account, password);
        ShowToast("√‹¬Î–ﬁ∏ƒ≥…π¶");
        ShowPanel(true, false, false);
    }

    #endregion

    #region UI Management

    private void ShowPanel(bool showLogin, bool showRegister, bool showChangePassword)
    {
        loginPanel.SetActive(showLogin);
        registerPanel.SetActive(showRegister);
        changePasswordPanel.SetActive(showChangePassword);
    }

    public void ShowToast(string content)
    {
        Debug.Log(content);
        toast.SetActive(true);
        toastContent.text = content;
        StartCoroutine(IEShowContent());
    }

    private IEnumerator IEShowContent()
    {
        yield return new WaitForSeconds(2);
        toast.SetActive(false);
    }

    #endregion
}
