using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Marks an object as destroyable and invokes an event when destroyed.
/// </summary>
public class Destroyable : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    public UnityEvent onDestroyed = new UnityEvent();

    public void Destroy()
    {
        onDestroyed.Invoke();
        Destroy(gameObject);
    }
}
