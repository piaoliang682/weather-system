using UnityEngine;

public class OpenWebsite : MonoBehaviour
{
    public string url;
    public void OpenWeb()
    {
        Application.OpenURL(url);
    }
}