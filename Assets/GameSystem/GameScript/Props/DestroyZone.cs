using UnityEngine;
using UnityEngine.Events;

public class DestroyZone : MonoBehaviour
{
    [Header("Destroy Threshold")]
    [SerializeField]
    private int destroyThreshold = 5;

    [Header("Events")]
    [SerializeField]
    public UnityEvent onThresholdReached = new UnityEvent();

    private int destroyCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        Destroyable destroyable = other.GetComponent<Destroyable>();
        if (destroyable != null)
        {
            destroyCount++;
            destroyable.Destroy();

            if (destroyCount >= destroyThreshold)
            {
                onThresholdReached.Invoke();
                Debug.Log($"[DestroyZone] Destroy threshold reached! ({destroyCount}/{destroyThreshold})");
                destroyCount = 0; // Reset counter after threshold
            }
        }
    }

    public void ResetDestroyCount()
    {
        destroyCount = 0;
    }

    public int GetDestroyCount() => destroyCount;
}

