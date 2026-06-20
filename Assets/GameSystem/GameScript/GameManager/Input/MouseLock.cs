using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家摄像机的逻辑  
/// 左右旋转 实现玩家左右旋转移动
/// 摄像机上下旋转控制视线上下旋转
/// </summary>
public class MouseLock : MonoBehaviour
{
    [Tooltip("鼠标灵敏度")] public float MouseSenstivity = 400f;


    public bool isCursorLock = true;
    private void Start()
    {
        if (isCursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;//鼠标指针隐藏并锁定
        Cursor.visible = false;
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            isCursorLock = !isCursorLock;
            Cursor.lockState = isCursorLock ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isCursorLock;
        }
    }
    
   

}

