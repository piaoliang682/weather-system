

using UnityEngine;

public class PaintRaycaster3D : IPaintRaycaster
{
    Camera cam;

    public PaintRaycaster3D(Camera camera)
    {
        cam = camera;
    }

    public bool Raycast(Vector2 screenPos, out PaintHit hit)
    {
        hit = new PaintHit();
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (!Physics.Raycast(ray, out RaycastHit h)) return false;

        hit.target = h.collider.gameObject;
        hit.uv = h.textureCoord;
        return true;
    }
}
