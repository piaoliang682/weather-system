
using UnityEngine;


// Eraser brush
[System.Serializable]
public class EraserBrush : BrushBase
{
    public override void SetPaint()
    {
        color = Color.clear;
        // Could add effects like soft edges later
    }
}