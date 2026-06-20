using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStatusPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image happinessImage;
    [SerializeField] private Image stressImage;
    [SerializeField] private Image guiltImage;   // fixed name: previously "guildImage"

    [Header("Settings")]
    [SerializeField] private string dayPrefix = "Day ";

    // Thresholds for color coding
    private const float lowThreshold = 30f;
    private const float highThreshold = 70f;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 🕒 Day
        if (GameReference.TempGameStats.TryGetValue("Day", out float day))
            dayText.text = $"{dayPrefix}{day+1}";

        // 💰 Coins
        if (GameReference.TempGameStats.TryGetValue("Coins", out float coins))
            coinText.text = coins.ToString();

        // 😊 Happiness
        if (GameReference.TempGameStats.TryGetValue("Happiness", out float happiness))
            SetImageAlpha(happinessImage, happiness);

        // 😖 Stress
        if (GameReference.TempGameStats.TryGetValue("Stress", out float stress))
            SetImageAlpha(stressImage, stress);

        // 😔 Guilt
        if (GameReference.TempGameStats.TryGetValue("Guilt", out float guilt))
            SetImageAlpha(guiltImage, guilt);
    }

    /// <summary>
    /// Adjusts the image transparency based on value (0–100).
    /// </summary>
    private void SetImageAlpha(Image img, float value)
    {
        if (img == null)
            return;

        float alpha = Mathf.Clamp01(value / 100f); // 0–1 range
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    private string FormatStat(string name, float value)
    {
        string colorTag = GetColorTag(value);
        return $"{name}: <color={colorTag}>{value:F0}</color>";
    }

    private string GetColorTag(float value)
    {
        if (value < lowThreshold)
            return "#00FF00"; // Green
        else if (value > highThreshold)
            return "#FFD700"; // Yellow
        else
            return "#FFFFFF"; // White
    }
}
