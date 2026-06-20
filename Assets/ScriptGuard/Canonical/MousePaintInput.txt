using UnityEngine;


public class MousePaintInput : IPaintInput
{
    public bool IsPainting() => Input.GetMouseButton(0);
    public Vector2 GetScreenPosition() => Input.mousePosition;
}
