using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class CropGrowth : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField] private string categoryName = "CropStage";
    [SerializeField] private string[] stageVisualIDs;
    [SerializeField] private float growTimePerStage = 10f;

    private int currentStage = 0;
    private bool isWatered = false;
    private float growTimer = 0f;

    private VisualController visualController;

    public UnityEvent onFullyGrown;
    private void Awake()
    {

        visualController = GetComponent<VisualController>();


    }

    private void Start()
    {
          UpdateVisual();      
    }

    private void Update()
    {
        if (!isWatered)
            return;

        growTimer += Time.deltaTime;

        if (growTimer >= growTimePerStage)
        {
            AdvanceStage();
        }
    }

    /// <summary>
    /// Called when player waters the crop
    /// </summary>
    public void Water()
    {
        if (isWatered)
            return;

        if (currentStage >= stageVisualIDs.Length - 1)
            return; // Fully grown

        isWatered = true;
        growTimer = 0f;

        Debug.Log("Crop watered");
    }

    private void AdvanceStage()
    {

        isWatered = false;
        growTimer = 0f;
        currentStage++;

        UpdateVisual();

        Debug.Log($"Crop grew to stage {currentStage}");
        if (IsFullyGrown())
        {
            onFullyGrown.Invoke();
        }

    }

    private void UpdateVisual()
    {
        if (visualController == null)
            return;

        string visualID = stageVisualIDs[currentStage];
        visualController.SetVisual(categoryName, visualID);
    }

    public bool IsFullyGrown()
    {
        return currentStage >= stageVisualIDs.Length - 1;
    }
}
