using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class LoadScene : MonoBehaviour
{
    [Header("Scene Loading Settings")]
    public string sceneName;
    public string loadingScene;
    public float delay = 0.2f;


    // =========================
    // PUBLIC API
    // =========================

    public void Load()
    {
        StartCoroutine(RunLoad(sceneName, loadingScene!=""));
    }



    public void LoadByName(string name)
    {
        StartCoroutine(RunLoad(name, false));
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(RunLoad(SceneManager.GetActiveScene().name, false));
    }

    public void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("LoadScene: No next scene available.");
            return;
        }

        string path = SceneUtility.GetScenePathByBuildIndex(nextIndex);
        string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(path);

        StartCoroutine(RunLoad(nextSceneName, false));
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // =========================
    // CORE LOADER
    // =========================

    private IEnumerator RunLoad(string targetScene, bool useLoadingScene)
    {
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("LoadScene: target scene is empty.");
            yield break;
        }

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (useLoadingScene && !string.IsNullOrEmpty(loadingScene))
        {
            // Load loading scene first
            yield return SceneManager.LoadSceneAsync(loadingScene);

            // Start loading target scene
            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
            op.allowSceneActivation = false;

            // Wait until almost done
            while (op.progress < 0.9f)
                yield return null;

            // Small buffer (optional polish)
            yield return new WaitForSeconds(0.3f);

            op.allowSceneActivation = true;
        }
        else
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);

            while (!op.isDone)
                yield return null;
        }
    }
}