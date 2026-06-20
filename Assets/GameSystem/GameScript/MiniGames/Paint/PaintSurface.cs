using UnityEngine;
using UnityEngine.Events;

public class PaintSurface : MonoBehaviour
{
    [SerializeField]
    private bool isResetOnStart;
    public Texture2D referenceTexture;
    public Texture2D texture;
    SpriteRenderer sr;
    Renderer rendererTarget;
    public UnityEvent onFullyDrawn;

    private int allPixelCount;
    private int drawnPixelCount=0;
    void Awake()
    {
        if (isResetOnStart)
        {
            ResetTexture();

        }
        ApplyReferencedTexture();

        allPixelCount = GetAllPixel();
        //Debug.Log(allPixelCount);

        //sr = GetComponent<SpriteRenderer>();
        ////Sprite sprite = sr.sprite;
        //Texture2D source = rendererTarget.material.mainTexture as Texture2D;

        //// ✅ Create a CPU-writable texture
        //texture = new Texture2D(
        //    (int)sprite.rect.width, 
        //    (int)sprite.rect.height,
        //    TextureFormat.RGBA32,
        //    false
        //);

        //// Copy pixels from original sprite region
        //Color[] pixels = source.GetPixels(
        //    (int)sprite.rect.x,
        //    (int)sprite.rect.y,
        //    (int)sprite.rect.width,
        //    (int)sprite.rect.height
        //);
        //texture.SetPixels(pixels);

        //texture.Apply();

        //// Recreate sprite with new texture
        //sr.sprite = Sprite.Create(
        //    texture,
        //    new Rect(0, 0, texture.width, texture.height),
        //    sprite.pivot / sprite.rect.size,
        //    sprite.pixelsPerUnit
        //);

    }

    public void Paint(Vector2 uv, BrushBase brush)
    {
        int cx = (int)(uv.x * texture.width);
        int cy = (int)(uv.y * texture.height);
        DrawCircle(cx, cy, brush);
        texture.Apply();
        float value = (GetNonOriginalPixelCount()) / (float)allPixelCount;

        //Debug.Log($"paint on obj named {gameObject.name}, value: {value}");
        if (value >= 0.9f)
            onFullyDrawn?.Invoke();
    }

    void DrawCircle(int cx, int cy, BrushBase brush)
    {
        Color col = brush.GetFinalColor();
        for (int x = -brush.size; x <= brush.size; x++)
        {
            for (int y = -brush.size; y <= brush.size; y++)
            {
                if (x * x + y * y > brush.size * brush.size) continue;
                int px = cx + x;
                int py = cy + y;
                if (px < 0 || px >= texture.width || py < 0 || py >= texture.height) continue;
                texture.SetPixel(px, py, col);
            }
        }
    }

    // Count non-transparent pixels
    public int CountNonTransparentPixels()
    {
        Color[] pixels = texture.GetPixels();
        int count = 0;
        foreach (Color c in pixels)
        {
            if (c.a > 0f) count++;
        }
        return count;
    }
    public int GetAllPixel()
    {
        Color[] currentPixels = texture.GetPixels();
        return currentPixels.Length;
    }
    public int GetNonOriginalPixelCount()
    {
        Color[] currentPixels = texture.GetPixels();
        Color[] referencePixels = referenceTexture.GetPixels();

        int count = 0;
        float tolerance = 0.05f;

        int length = Mathf.Min(currentPixels.Length, referencePixels.Length);

        for (int i = 0; i < length; i++)
        {
            if (!IsSameColor(currentPixels[i], referencePixels[i], tolerance))
            {
                count++;
            }
        }

        return count;
    }

    bool IsSameColor(Color a, Color b, float tolerance)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }


    // Count white pixels (with tolerance)
    public int GetWhitePixelCount()
    {
        Color[] pixels = texture.GetPixels();
        int count = 0;
        foreach (Color c in pixels)
        {
            if (IsWhite(c)) count++;
        }
        return count;
    }

    public void FinishDraw()
    {onFullyDrawn?.Invoke();

    }

    // Reset all pixels to white
    public void ResetTexture()
    {
        Color[] whitePixels = new Color[texture.width * texture.height];
        for (int i = 0; i < whitePixels.Length; i++)
        {
            whitePixels[i] = Color.white;
            whitePixels[i].a = 1;
        }
        // Copy pixels from original sprite region

        texture.SetPixels(whitePixels);
        texture.Apply();
    }
    public void ApplyReferencedTexture()
    {
        //Color[] whitePixels = new Color[Referencetexture.width * Referencetexture.height];
        //for (int i = 0; i < whitePixels.Length; i++)
        //{
        //    whitePixels[i] = Color.white;
        //}
        // Copy pixels from original sprite region
        Color[] whitePixels = referenceTexture.GetPixels(); // returns all pixels in row-major order
        texture.SetPixels(whitePixels);
        texture.Apply();
    }
    // Helper to consider "white" (tolerance for float precision)
    bool IsWhite(Color c)
    {
        float tolerance = 0.1f;
        return Mathf.Abs(c.r - 1f) < tolerance &&
               Mathf.Abs(c.g - 1f) < tolerance &&
               Mathf.Abs(c.b - 1f) < tolerance &&
               c.a > 0f;
    }
}
