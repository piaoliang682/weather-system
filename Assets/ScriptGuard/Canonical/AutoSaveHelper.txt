using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;

[InitializeOnLoad]
public static class AutoSaveHelper
{
    // Auto save every X minutes
    private const double AutoSaveIntervalMinutes = 5;

    // Show notification
    private const bool ShowNotification = true;

    private static double nextSaveTime;

    static AutoSaveHelper()
    {
        nextSaveTime = EditorApplication.timeSinceStartup + AutoSaveIntervalMinutes * 60;

        EditorApplication.update += Update;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;

        Debug.Log("[AutoSaveHelper] Enabled.");
    }

    private static void Update()
    {
        // Don't autosave during play mode
        if (EditorApplication.isPlaying)
            return;

        // Wait until timer reached
        if (EditorApplication.timeSinceStartup < nextSaveTime)
            return;

        SaveAll();

        nextSaveTime = EditorApplication.timeSinceStartup + AutoSaveIntervalMinutes * 60;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        // Auto save before entering play mode
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            SaveAll();
        }
    }

    private static void SaveAll()
    {
        // Save scenes
        EditorSceneManager.SaveOpenScenes();

        // Save assets
        AssetDatabase.SaveAssets();

        Debug.Log($"[AutoSaveHelper] Auto saved at {DateTime.Now:HH:mm:ss}");

        if (ShowNotification)
        {
            ShowEditorNotification("Auto Saved Scenes");
        }
    }

    private static void ShowEditorNotification(string message)
    {
        EditorWindow window = EditorWindow.focusedWindow;

        if (window != null)
        {
            window.ShowNotification(new GUIContent(message));
        }
    }

    [MenuItem("Tools/Auto Save/Save Now %#s")]
    private static void ManualSave()
    {
        SaveAll();
    }
}