using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public Camera captureCamera; // 拖入用于截图的相机
    public Vector3 offset = new Vector3(0, 5, -10); // 相机相对于目标的偏移位置
    public float fieldOfView = 60f; // 相机视角参数

    public string folderName = "CapturedImages"; // 存放文件夹名

    void Start()
    {
        if (captureCamera == null) captureCamera = Camera.main;
        
        // 确保文件夹存在
        string path = Path.Combine(Application.dataPath, folderName);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    void Update()
    {
        // 1. 调整相机参数 (作业要求：相机参数可调整)
        captureCamera.fieldOfView = fieldOfView;

        // 2. 捕捉图像 (按键 C 进行捕获)
        if (Input.GetKeyDown(KeyCode.C))
        {
            CaptureAndSave();
        }
    }

    public void CaptureAndSave()
    {
        // 设置保存路径 (作业要求：存放在指定位置)
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filePath = Path.Combine(Application.dataPath, folderName, "Capture_" + timestamp + ".png");

        // 执行截图
        ScreenCapture.CaptureScreenshot(filePath);
        
        Debug.Log("图像已捕获并保存至: " + filePath);
    }
}