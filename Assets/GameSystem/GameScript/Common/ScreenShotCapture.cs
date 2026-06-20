using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ScreenshotCapture : MonoBehaviour
{
    public Button captureButton;

    void Start()
    {
        if (captureButton != null)
        {
            captureButton.onClick.AddListener(TakePicture);
        }
    }
    public void TakePicture()
    {
        StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        if (captureButton != null)
            captureButton.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        string downloadsPath = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "Downloads"
        );

        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(downloadsPath, fileName);

        ScreenCapture.CaptureScreenshot(fullPath);

        Debug.Log("Screenshot saved to: " + fullPath);

        yield return null;

        if (captureButton != null)
            captureButton.gameObject.SetActive(true);
    }
}