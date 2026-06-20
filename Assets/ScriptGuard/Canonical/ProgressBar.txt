using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class ProgressBar : MonoBehaviour
{
    [Header("Dialogue")]
    public string[] progressDialogue;
    public TMP_Text dialogueText;

    [Header("UI")]
    public TMP_Text progressText;
    public Image image; // Assign in Inspector

    [Header("Color")]
    public Color emptyColor = Color.red;
    public Color fullColor = Color.green;

    private float previousFill = -1f;

    void Update()
    {
        if (!Application.isPlaying)
        {
            ApplyIfChanged();
        }
    }

    void LateUpdate()
    {
        if (Application.isPlaying)
        {
            ApplyIfChanged();
        }
    }

    /// <summary>
    /// progress: 0.0 to 1.0 inclusive
    /// </summary>
    public void SetImageProgress(float progress)
    {
        if (image == null)
            return;

        float clamped = Mathf.Clamp01(progress);
        image.fillAmount = clamped;

        ApplyAll(true);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        ApplyAll(true);
    }
#endif

    private void ApplyIfChanged()
    {
        if (image == null) return;

        float currentFill = Mathf.Clamp01(image.fillAmount);
        if (Mathf.Approximately(previousFill, currentFill)) return;

        ApplyAll();
    }

    private void ApplyAll(bool force = false)
    {
        if (image == null) return;

        float fill = Mathf.Clamp01(image.fillAmount);

        // Color
        image.color = Color.Lerp(emptyColor, fullColor, fill);

        // Progress text (percentage)
        if (progressText != null)
        {
            progressText.text = Mathf.RoundToInt(fill * 100f) + "%";
        }

        // Dialogue
        if (dialogueText != null && progressDialogue != null && progressDialogue.Length > 0)
        {
            int index = Mathf.FloorToInt(fill * progressDialogue.Length);
            index = Mathf.Clamp(index, 0, progressDialogue.Length - 1);
            dialogueText.text = progressDialogue[index];
        }

        previousFill = fill;

#if UNITY_EDITOR
        if (!Application.isPlaying)
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
#endif
    }
}
