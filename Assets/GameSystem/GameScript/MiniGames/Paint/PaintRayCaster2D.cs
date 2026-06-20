using UnityEngine;

public class PaintRayCaster2D : IPaintRaycaster
{
    Camera cam;

    public PaintRayCaster2D(Camera camera)
    {
        cam = camera;
    }

    public bool Raycast(Vector2 screenPos, out PaintHit hit)
    {
        hit = new PaintHit();

        Vector2 worldPos = cam.ScreenToWorldPoint(screenPos);
        RaycastHit2D h = Physics2D.Raycast(worldPos, Vector2.zero);

        if (!h.collider) return false;

        hit.target = h.collider.gameObject;
        hit.uv = WorldToSpriteUV(h);

        return true;
    }

    private Vector2 WorldToSpriteUV(RaycastHit2D hit)
    {
        SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
        if (sr == null) return Vector2.zero;

        Sprite sprite = sr.sprite;

        // Convert world point ¡ú local
        Vector2 localPos = hit.collider.transform.InverseTransformPoint(hit.point);

        // Normalize to sprite space
        Vector2 pivot = sprite.pivot;
        Vector2 pixelsPerUnit = sprite.pixelsPerUnit * Vector2.one;

        Vector2 pixelPos = new Vector2(
            pivot.x + localPos.x * pixelsPerUnit.x,
            pivot.y + localPos.y * pixelsPerUnit.y
        );

        return new Vector2(
            pixelPos.x / sprite.rect.width,
            pixelPos.y / sprite.rect.height
        );
    }
}
