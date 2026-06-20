using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
public class BookPanel : InteractivePanelBase
{
    [Header("Page Content References")]
    [SerializeField] private TMP_Text topic;
    [SerializeField] private TMP_Text title;
    [SerializeField] private List<TMP_Text> contentTexts = new List<TMP_Text>();
    [SerializeField] private List<Image> contentImages = new List<Image>();
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject imageParent;
    [SerializeField] private Transform gameObjectParent;
    [Header("Data")]
    [SerializeField] private ContentListSO contentListSO;
    [Header("Auto Flip Settings")]
    [SerializeField] private bool enableAutoFlip = false;
    [SerializeField] private float autoFlipInterval = 3f;
    [Header("Navigation Settings")]
    [SerializeField] private bool allowNextButton = true;
    [SerializeField] private bool allowPrevButton = true;
    [Header("Events")]
    public UnityEvent onBookEnd;

    private int currentIndex = 0;
    private Coroutine autoFlipCoroutine;

    private void Start()
    {
        SetContent(currentIndex);
        if (nextButton == null) return;
        prevButton.onClick.AddListener(OnPrevClicked);
        nextButton.onClick.AddListener(OnNextClicked);
        StartAutoFlip();
    }


    private void OnEnable()
    {
            SetContent(currentIndex);
            StartAutoFlip();
    }

    public void SetContentListSO(ContentListSO so)
    {
        contentListSO = so;
    }

    public void ResetContent()
    {
        //ShowPanel();
        currentIndex = 0;
        SetContent(currentIndex);
        ShowPanel();
    }
    private void SetContent(int index)
    {

        if (contentListSO == null)
        {
            Debug.Log("contentListSO");
            return;
        }
            // Clamp index
            currentIndex = Mathf.Clamp(index, 0, Mathf.Max(0, contentListSO.contentListSO.Count - 1));

        // Update topic
        if (title != null)
            title.text = contentListSO.topic;

        // --- Update all text fields ---

        if (contentTexts != null && contentTexts.Count > 0)
        {
            // Get current content data safely
            var currentContent = contentListSO.contentListSO[currentIndex];

            for (int i = 0; i < contentTexts.Count; i++)
            {

                TMP_Text textField = contentTexts[i];
                if (textField == null) continue;
                if (currentContent.topic != null)
                    topic.text = currentContent.topic;
                if (currentContent.textList != null && i < currentContent.textList.Count)
                {
                    textField.text = currentContent.textList[i];
                }
                else if (currentContent.textList != null && currentIndex < currentContent.textList.Count)
                {
                    // fallback to the currentIndex if we have fewer text fields
                    textField.text = currentContent.textList[currentIndex];
                }
                else
                {
                    textField.text = "";
                }
            }
        }

        if(imageParent!=null) imageParent.SetActive(false);
        foreach (Image img in contentImages)
        {

            img.gameObject.SetActive(false);
        }
        if (contentImages != null && contentImages.Count > 0)
        {
            var currentContent = contentListSO.contentListSO[currentIndex];

            for (int i = 0; i < contentImages.Count; i++)
            {
            //imageParent.SetActive(true);
                Image img = contentImages[i];
                if (img == null) continue;

                if (currentContent.spriteList != null && i < currentContent.spriteList.Count)
                {
                    if (imageParent != null) imageParent.SetActive(true);
                    img.gameObject.SetActive(true);
                    img.sprite = currentContent.spriteList[i];
                }
                else
                {
                    img.gameObject.SetActive(false);
                    img.sprite = null;
                }
            }

        }

        // Clear existing children
        foreach (Transform child in gameObjectParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject gb in contentListSO.contentListSO[currentIndex].gameObjects)
        {
            gameObjectParent.gameObject.SetActive(true);
            GameObject instance = Instantiate(gb, gameObjectParent.transform);
        }
        if (contentListSO.contentListSO[currentIndex].gameObjects.Count <= 0)
        {
            gameObjectParent.gameObject.SetActive(false);
        }
        // --- Update all sprite images ---


        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        // Prev button
        if (prevButton != null)
        {
            bool showPrev = allowPrevButton && currentIndex > 0;
            prevButton.gameObject.SetActive(showPrev);
        }

        // Next button
        if (nextButton != null)
        {
            bool showNext = allowNextButton &&
                            currentIndex < contentListSO.contentListSO.Count - 1;

            nextButton.gameObject.SetActive(showNext);
        }

        // Invoke end event if at last page
        if (currentIndex >= contentListSO.contentListSO.Count - 1)
        {
            onBookEnd?.Invoke();
        }
    }
    public void OnNextClicked()
    {
        if (currentIndex < contentListSO.contentListSO.Count - 1)
        {

            currentIndex++;
            Debug.Log(currentIndex);

            SetContent(currentIndex);
        }
    }

    public void OnPrevClicked()
    {
        if (currentIndex > 0)
        {

            currentIndex--;
            Debug.Log(currentIndex);
            SetContent(currentIndex);
        }
    }

    private void StartAutoFlip()
    {
        StopAutoFlip();
        if (enableAutoFlip && contentListSO != null && contentListSO.contentListSO.Count > 1)
        {
            autoFlipCoroutine = StartCoroutine(AutoFlipCoroutine());
        }
    }

    private void StopAutoFlip()
    {
        if (autoFlipCoroutine != null)
        {
            StopCoroutine(autoFlipCoroutine);
            autoFlipCoroutine = null;
        }
    }

    private IEnumerator AutoFlipCoroutine()
    {
        while (enableAutoFlip)
        {
            yield return new WaitForSeconds(autoFlipInterval);
            if (enableAutoFlip && currentIndex < contentListSO.contentListSO.Count - 1)
            {
                OnNextClicked();
            }
        }
    }

    public void SetAutoFlip(bool enable, float interval = -1f)
    {
        enableAutoFlip = enable;
        if (interval > 0f)
        {
            autoFlipInterval = interval;
        }
        if (enable)
        {
            StartAutoFlip();
        }
        else
        {
            StopAutoFlip();
        }
    }

    private void OnDisable()
    {
        StopAutoFlip();
    }

    public void ShowBookPanel(int contentIndex = 0)
    {
        currentIndex = contentIndex;
        SetContent(currentIndex);
        ShowPanel();
    }

    public void HideBookPanel()
    {
        ClosePanel();
    }

    public void ShowTemp(float sec)
    {
        StartCoroutine(ShowTempRoutine( sec));
    }

    private IEnumerator ShowTempRoutine(float sec)
    {
        // Activate panel
        ShowPanel();
        Debug.Log("[BookPanel] Panel activated for temporary display");

        // Play fade
        UIFadeInOut fadeInOut = GetComponent<UIFadeInOut>();

        if (fadeInOut != null)
        {
            fadeInOut.FadeInThenOut();
            Debug.Log("[BookPanel] Called FadeInThenOut");
        }
        else
        {
            Debug.LogWarning("[BookPanel] UIFadeInOut component not found");
        }

        // Wait a few seconds
        yield return new WaitForSeconds(sec);

        // Close panel after delay
        ClosePanel();
        Debug.Log("[BookPanel] Panel disabled after showing temp");
    }

    public void SetNavigation(bool allowPrev, bool allowNext)
    {
        allowPrevButton = allowPrev;
        allowNextButton = allowNext;

        UpdateButtonState();
    }
}
