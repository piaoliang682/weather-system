using UnityEngine;
using DG.Tweening;

public class TranslationAnimation : MonoBehaviour
{
    [Header("Move Settings")]
    public Transform startTransform;    // Starting position
    public Transform targetTransform;   // Target position
    public float duration = 2f;         // Duration of move

    [Header("Play Options")]
    public bool playOnStart = true;     // Should play automatically?

    private Tweener moveTween;

    void Start()
    {
        if (playOnStart)
            PlayMove();
    }

    public void PlayMove()
    {
        if (startTransform == null || targetTransform == null)
        {
            Debug.LogWarning("Start or Target Transform not assigned!");
            return;
        }

        // Place object at start
        transform.position = startTransform.position;

        // Create a tween to move to target over duration
        moveTween = transform.DOMove(targetTransform.position, duration)
                             .SetEase(Ease.Linear);   // Optional easing
    }
}
