using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class EnvironmentPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button weatherButton;
    public Button timeButton;

    public UnityEvent onWeatherChanged;
    public UnityEvent onSpecificWeatherChanged;
    public int targetWeatherIndex = 0;
    public int targetTimeIndex = 0;

    private int currentWeatherIndex = 0;
    private int currentTimeIndex = 0;


    private void Start()
    {
        if (weatherButton != null)
            weatherButton.onClick.AddListener(ChangeWeather);

        if (timeButton != null)
            timeButton.onClick.AddListener(ChangeTime);
    }

    private void ChangeWeather()
    {
        var env = EnvironmentManager.Instance;
        if (env == null || env.weatherRegistry == null) return;

        currentWeatherIndex++;
        if (currentWeatherIndex >= env.weatherRegistry.weatherProfiles.Count)
            currentWeatherIndex = 0;

        env.SetWeather(env.weatherRegistry.weatherProfiles[currentWeatherIndex]);
        CheckSpecificWeatherTime();
    }

    private void ChangeTime()
    {
        var env = EnvironmentManager.Instance;
        if (env == null || env.timeRegistry == null) return;

        currentTimeIndex++;
        if (currentTimeIndex >= env.timeRegistry.timesOfDayProfiles.Count)
            currentTimeIndex = 0;

        env.SetTimeOfDay(env.timeRegistry.timesOfDayProfiles[currentTimeIndex]);
        CheckSpecificWeatherTime();
    }

    private void CheckSpecificWeatherTime()
    {
        if (currentWeatherIndex == targetWeatherIndex && currentTimeIndex==targetTimeIndex)
        {
            onSpecificWeatherChanged.Invoke();
        }
    }
}
