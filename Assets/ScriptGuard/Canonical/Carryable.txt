using UnityEngine;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Marks an object as carryable by the player.
/// </summary>
public class Carryable : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    public UnityEvent onCarried = new UnityEvent();
    [SerializeField]
    public UnityEvent onDropped = new UnityEvent();

    public void OnCarried()
    {
        onCarried.Invoke();
    }

    public void OnDropped()
    {
        onDropped.Invoke();
    }
}
