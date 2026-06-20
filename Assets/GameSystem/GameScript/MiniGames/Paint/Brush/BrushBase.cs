using UnityEngine;

// Base brush class
[System.Serializable]
public abstract class BrushBase
{
    public Color color = Color.red;
    public int size = 8;

    // Get the color to apply at a pixel
    public virtual Color GetFinalColor() {


        SetPaint();
        return color;
    }

    // Optional: update or randomize brush behavior
    public virtual void SetPaint() 
    {

    }
}
