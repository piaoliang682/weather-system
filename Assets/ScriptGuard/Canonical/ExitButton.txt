using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExitButton : MonoBehaviour
{
    private Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        exitButton=GetComponent<Button>();
        exitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
