using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public Text timerText;      // Assign the Text in the Inspector
    public Image image;         // Assign the circular fillable Image
    public bool isStartedOnAwake = false;
    public float duration = 60f;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    public UnityEvent OnTimeout;    // Event triggered when time runs out

    private void Start()
    {
        if (isStartedOnAwake)
        {
            ResetTimer();
            StartTimer();
        }
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= duration)
        {
            elapsedTime = duration;
            isRunning = false;
            OnTimeout?.Invoke();  // Trigger timeout
        }

        UpdateTimerDisplay();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;
        UpdateTimerDisplay();
    }
    public void SetMaxTime(float time)
    {
        duration = Mathf.Max(0.1f, time);  // Prevent zero or negative duration
        UpdateTimerDisplay();              // Optional: immediately update the UI
    }

    public bool IsTimeUp()
    {
        return elapsedTime >= duration;
    }

    public void SetTime(float num)
    {
        elapsedTime = num;
    }

    public void AddTime(float num)
    {
        elapsedTime -= num;
    }
    public float GetElapsedTime() => Mathf.Clamp(elapsedTime, 0f, duration);
    public float GetRemainingTime() => Mathf.Clamp(duration - elapsedTime, 0f, duration);
    public bool IsStarted() => isRunning;

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(GetRemainingTime()).ToString();
        }

        if (image != null)
        {
            image.fillAmount = GetRemainingTime() / duration;
        }
    }
}
