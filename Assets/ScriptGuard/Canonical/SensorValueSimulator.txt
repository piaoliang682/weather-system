using UnityEngine;
using TMPro; // optional, if you want to display value

public class SensorValueSimulator : MonoBehaviour
{
    [Header("Sensor Settings")]
    public float initialValue = 50f;      // starting value
    public float minValue = 0f;           // minimum possible value
    public float maxValue = 100f;         // maximum possible value
    public float maxChangePerSecond = 2f; // max change per second
    public bool updateEveryFrame = true;  // update every frame or by fixed interval
    public float updateInterval = 0.1f;   // if not every frame, seconds between updates

    [Header("UI Display (Optional)")]
    public TMP_Text valueText;

    private float currentValue;
    private float timer = 0f;

    void Start()
    {
        currentValue = Mathf.Clamp(initialValue, minValue, maxValue);

        if (valueText != null)
            valueText.text = currentValue.ToString("F2");
    }

    void Update()
    {
        if (updateEveryFrame)
        {
            UpdateSensorValue(Time.deltaTime);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                UpdateSensorValue(timer);
                timer = 0f;
            }
        }

        if (valueText != null)
            valueText.text = currentValue.ToString("F2");
    }

    private void UpdateSensorValue(float deltaTime)
    {
        // Random float change per update
        float change = Random.Range(-maxChangePerSecond, maxChangePerSecond) * deltaTime;
        currentValue += change;

        // Clamp value within range
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
    }

    // Optional: get current simulated value
    public float GetValue()
    {
        return currentValue;
    }
}
