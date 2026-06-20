using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    private bool mIsFirstEnter;


    private void OnMouseEnter()
    {
        if (mIsFirstEnter) return;
        //AudioManager.Instance.Play("hover");
        mIsFirstEnter = true;
        transform.DOLocalRotate(Vector3.up * 180, 1);
    }
    private void OnMouseDown()
    {
            //AudioManager.Instance.Play("click");

    }
}
