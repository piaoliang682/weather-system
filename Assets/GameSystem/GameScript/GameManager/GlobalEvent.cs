// GlobalEvent.cs
using UnityEngine;
using UnityEngine.Events;

public class GlobalEvent : MonoBehaviour
{
    public static GlobalEvent Instance { get; private set; }

    // Example: an event for ¡°player entered zone¡±, no parameter
    public static UnityEvent OnPlayerInZone;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (OnPlayerInZone == null)
            OnPlayerInZone = new UnityEvent();
    }

    public void RaisePlayerEnteredZone()
    {
        OnPlayerInZone.Invoke();
    }

    // If you need variants with parameters, you can define UnityEvent<T>, 
    // or custom UnityEvent subclass (see Unity docs). :contentReference[oaicite:2]{index=2}
}
