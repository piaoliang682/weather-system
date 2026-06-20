using UnityEngine;

public class WaterCrop2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        CropGrowth crop = other.GetComponent<CropGrowth>();
        if (crop != null)
        {
            Debug.Log("watering");
            crop.Water();
        }
    }
}
