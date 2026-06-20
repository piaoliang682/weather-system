using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationButton : MonoBehaviour
{
    [Header("UI")]
    public Button triggerButton;
    public TMP_Text countdownText;

    [Header("Animation Targets")]
    public GameObject parentObject;   // Parent GameObject to find all child Animators

    [Header("Settings")]
    public float delaySeconds = 3f;

    private List<Animator> targetAnimators = new List<Animator>();
    private Coroutine countdownRoutine;
    private bool isActive = true;     // tracks current state

    private void Start()
    {
        if (triggerButton != null)
            triggerButton.onClick.AddListener(OnButtonClicked);

        if (countdownText != null)
            countdownText.text = "";

        // Get all Animator components in children of parentObject
        if (parentObject != null)
        {
            targetAnimators = new List<Animator>(parentObject.GetComponentsInChildren<Animator>());
        }
    }

    private void OnButtonClicked()
    {
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(DelayedToggleAnimation());
    }

    private IEnumerator DelayedToggleAnimation()
    {
        float timeLeft = delaySeconds;

        while (timeLeft > 0)
        {
            if (countdownText != null)
                countdownText.text = Mathf.Ceil(timeLeft).ToString();

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        if (countdownText != null)
            countdownText.text = "";

        ToggleAnimation();
    }

    private void ToggleAnimation()
    {
        isActive = !isActive;

        foreach (var animator in targetAnimators)
        {
            if (animator != null)
            {
                animator.speed = isActive ? 1f : 0f; // resume or pause
            }
        }
    }
}
