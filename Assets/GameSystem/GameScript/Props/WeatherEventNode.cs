using UnityEngine;

public class WeatherEventNode : InteractableBase
{
    [Tooltip("Weather profile name (must exist in the EnvironmentManager registry).")]
    public string weatherName;
    [Tooltip("Audio definition name (from AudioRegistry).")]
    public string audioName;

    [Header("Gameplay Effect")]
    public string key = "Happiness";
    public int valueChange = -10;
    public float eventLifetime = 5f;

    private void Start()
    {
    }
    protected override void OnConfirmed()
    {
        // Set node lifetime
        lifeTime = eventLifetime;

        // Apply happiness penalty
        if (GameReference.TempGameStats.ContainsKey(key))
        {
            GameReference.TempGameStats["Happiness"] += valueChange;

            // Choose color based on value sign
            string color = valueChange >= 0 ? "green" : "red";

            // Add to confirm feed
            confirmFeed += $"\n <color={color}>{key} {valueChange:+#;-#;0}</color>";
        }

        // Trigger weather change
        if (EnvironmentManager.Instance != null)
        {
            EnvironmentManager.Instance.SetWeatherByName(weatherName);
        }
        else
        {
            Debug.LogWarning("EnvironmentManager not found in scene.");
        }

        // Play event sound
        if (GameReference.AmbienceAudioSource != null && !string.IsNullOrEmpty(audioName))
        {
            var audioDef = GameReference.AudioRegistry.GetDefinition(audioName);
            if (audioDef != null && audioDef.clip != null)
            {
                GameReference.AmbienceAudioSource.clip = audioDef.clip;
                GameReference.AmbienceAudioSource.Play();
            }
        }

        base.OnConfirmed();
    }
}
