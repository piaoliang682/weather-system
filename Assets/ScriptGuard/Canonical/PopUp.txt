using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PopUp : MonoBehaviour
{
    [Header("Popup Elements")]
    [SerializeField] private TMP_Text popupText;

    [Header("Settings")]
    [SerializeField] private float lifetime = 5f;      // how long before starting fade
    [SerializeField] private float fadeDuration = 1f; // fade out time

    private CanvasGroup canvasGroup;
    //private float timer = 0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (popupText == null)
            popupText = GetComponentInChildren<TMP_Text>();

        canvasGroup.alpha = 1f;
    }

    private void Start()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    public float GetTotalLife()
    {
        return lifetime + fadeDuration;
    }

    public void SetLifeTime(float t)
    {
        lifetime =t;
    }
    public void SetMessage(string message)
    {
        if (popupText != null)
            popupText.text = message;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // Wait for the visible lifetime
        yield return new WaitForSeconds(lifetime);

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        // Smooth fade
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
