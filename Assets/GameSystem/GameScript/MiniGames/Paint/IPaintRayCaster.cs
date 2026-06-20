
using UnityEngine;


public interface IPaintRaycaster
{
    bool Raycast(Vector2 screenPos, out PaintHit hit);
}

public struct PaintHit
{
    public Vector2 uv;
    public GameObject target;
}
