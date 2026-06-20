using UnityEngine;

using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class UIFadeInOut : MonoBehaviour
{
    public enum FadeStartMode
    {
        FadeIn,
        FadeOut,
        FadeInThenOut
    }
    [Header("Start Behavior")]
    [SerializeField] private FadeStartMode startMode = FadeStartMode.FadeInThenOut;
    [Header("Fade Settings")]
    [SerializeField] private float fadeInDelay = 0f;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDelay = 0f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("Events")]
    public UnityEvent onFadeInComplete;
    public UnityEvent onFadeOutComplete;
    public UnityEvent onFadeInThenOutComplete;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (playOnStart)
        {

            switch (startMode)
            {

                case FadeStartMode.FadeIn:
                    FadeIn();
                    break;

                case FadeStartMode.FadeOut:
                    FadeOut();
                    break;

                case FadeStartMode.FadeInThenOut:
                    FadeInThenOut();
                    break;
            }
        }
    }


    /// <summary>
    /// Fade in only with completion callback
    /// </summary>
    public void FadeIn()
    {
        gameObject.SetActive(true);
        Debug.Log($"fade in{canvasGroup.name}");
        canvasGroup.DOKill();

        canvasGroup.alpha = 0f;

        canvasGroup.DOFade(1f, fadeInDuration)
            .SetDelay(fadeInDelay)
            .SetEase(Ease.InOutQuad);
    }


    public void FadeOut()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        Debug.Log("fade out");
        canvasGroup.DOKill();

        canvasGroup.DOFade(0f, fadeOutDuration)
            .SetDelay(fadeOutDelay)
            .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
    }



    /// <summary>
    /// Fade in then fade out with separate delays and durations
    /// </summary>
    public void FadeInThenOut()
    {
        canvasGroup.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(canvasGroup.DOFade(1f, fadeInDuration)
            .SetEase(Ease.InOutQuad));

        seq.AppendInterval(fadeOutDelay);

        seq.Append(canvasGroup.DOFade(0f, fadeOutDuration)
            .SetEase(Ease.InOutQuad));

        onFadeInThenOutComplete?.Invoke();
        Debug.Log("animation done");
    }
}
