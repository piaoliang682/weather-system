using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FreezeTime(float duration)
    {
        StartCoroutine(FreezeTimeCoroutine(duration));
    }

    private IEnumerator FreezeTimeCoroutine(float duration)
    {
        Time.timeScale = 0f; // Freeze time
        yield return new WaitForSecondsRealtime(duration); // Wait for the specified duration in real time
        Time.timeScale = 1f; // Resume time
    }
}
