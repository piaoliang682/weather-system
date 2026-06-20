using UnityEngine;

[System.Serializable]
public class RainbowBrush : BrushBase
{
    [Header("Rainbow Settings")]
    [Range(0f, 1f)] public float hueStep = 0.05f; // How much hue changes each step
    private float currentHue = 0f;

    /// <summary>
    /// Set the brush color to the next hue in the rainbow
    /// </summary>
    public override void SetPaint()
    {
        // Convert current hue to bright, fully saturated color
        color = Color.HSVToRGB(currentHue, 1f, 1f);

        // Increment hue and wrap around 1.0
        currentHue += hueStep;
        if (currentHue > 1f) currentHue -= 1f;
    }

    /// <summary>
    /// Optional: reset to start of hue
    /// </summary>
    public void ResetRainbow()
    {
        currentHue = 0f;
    }
}
