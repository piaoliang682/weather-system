using UnityEngine;
using UnityEngine.Events;

public class InputEvent : MonoBehaviour
{
    [Header("Keyboard Input")]
    public KeyCode keyboardKey = KeyCode.Space;

    [Header("Mouse Input (0=Left, 1=Right, 2=Middle)")]
    public int mouseButton = 0;

    [Header("Events")]
    public UnityEvent onKeyPressed;
    public UnityEvent onMousePressed;

    void Update()
    {
        if (Input.GetKeyDown(keyboardKey))
        {
            Debug.Log($"{keyboardKey} pressed");
            onKeyPressed?.Invoke();
        }

        if (Input.GetMouseButtonDown(mouseButton))
        {
            Debug.Log($"Mouse button {mouseButton} pressed");
            onMousePressed?.Invoke();
        }
    }
}