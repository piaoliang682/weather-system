using UnityEngine;
using UnityEngine.Events;

public class PainterController : MonoBehaviour
{
    public UnityEvent onFullyDrawn;

    IPaintInput input;
    IPaintRaycaster raycaster;

    public enum BrushType
    {
        Paint,
        Rainbow,
        Eraser
    }

    [Header("Brush Settings")]
    public BrushType brushType = BrushType.Paint; // pick in inspector
    public int brushSize = 8;                     // common size for all brushes
    public Color paintColor = Color.red;          // default paint color

    private BrushBase brush; // runtime brush instance

    public Camera cam;
    public bool is3D;
    public bool isStart = false;              // play painting on scene start



    void Start()
    {
        input = new MousePaintInput();

        // Setup raycaster
        raycaster = is3D ? (IPaintRaycaster)new PaintRaycaster3D(cam) : new PaintRayCaster2D(cam);

        // Create the brush based on the enum
        SetBrushByEnum(brushType);


    }

    void Update()
    {
        if (!isStart)
            return;
        if (!input.IsPainting() || brush == null)
            return;

        Vector2 screenPos = input.GetScreenPosition();

        if (!raycaster.Raycast(screenPos, out PaintHit hit))
            return;

        PaintSurface surface = hit.target.GetComponent<PaintSurface>();
        if (surface == null)
        {
            Debug.LogWarning("Hit object has NO PaintSurface");
            return;
        }

        surface.Paint(hit.uv, brush);

        float value = surface.GetNonOriginalPixelCount() / (float)surface.CountNonTransparentPixels();
        if (value >= 0.9f)
            onFullyDrawn?.Invoke();
    }

    public void CallStart()
    {
        isStart= true;  
    }
    /// <summary>
    /// Sets brush instance based on enum
    /// </summary>
    public void SetBrushByEnum(BrushType type)
    {
        switch (type)
        {
            case BrushType.Paint:
                //PaintBrush paintBrush = new PaintBrush();

                RainbowBrush paintBrush = new RainbowBrush();
                paintBrush.size = brushSize;
                paintBrush.color = paintColor;
                brush = paintBrush;
                break;
            case BrushType.Rainbow:
                RainbowBrush rainbowBrush = new RainbowBrush();
                rainbowBrush.size = brushSize;
                brush = rainbowBrush;
                break;
            case BrushType.Eraser:
                EraserBrush eraserBrush = new EraserBrush();
                eraserBrush.size = brushSize;
                brush = eraserBrush;
                break;
        }
    }

    /// <summary>
    /// Change brush at runtime
    /// </summary>
    public void SetBrush(BrushBase newBrush)
    {
        brush = newBrush;
    }
}
