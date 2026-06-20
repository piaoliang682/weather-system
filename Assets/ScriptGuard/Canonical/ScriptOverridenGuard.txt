using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class ScriptOverrideGuard
{
    // Canonical scripts stored inside Assets, but with .backup extension
    static string canonicalFolder = Path.Combine(Application.dataPath, "ScriptGuard", "Canonical");

    // Overridden versions stored inside Assets/ScriptGuard/Overrides
    static string overrideFolder = Path.Combine(Application.dataPath, "ScriptGuard", "Overrides");

    const float similarityThreshold = 0.1f; // <10% similarity = accidental override

    const string enabledKey = "ScriptOverrideGuard.Enabled";
    public static bool IsEnabled
    {
        get => EditorPrefs.GetBool(enabledKey, true);
        set => EditorPrefs.SetBool(enabledKey, value);
    }

    static ScriptOverrideGuard()
    {
        if (!Directory.Exists(canonicalFolder))
            Directory.CreateDirectory(canonicalFolder);

        if (!Directory.Exists(overrideFolder))
            Directory.CreateDirectory(overrideFolder);

        AssemblyReloadEvents.beforeAssemblyReload += GuardScripts;
    }

    static void GuardScripts()
    {
        if (!IsEnabled)
        {
            Debug.Log("ScriptOverrideGuard is disabled.");
            return;
        }
        Debug.Log("ScriptOverrideGuard is running.");
        string[] scripts = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories);

        foreach (string scriptPath in scripts)
        {
            string fileName = Path.GetFileName(scriptPath);
            string canonicalFileName = Path.ChangeExtension(fileName, ".txt");
            string canonicalPath = Path.Combine(canonicalFolder, canonicalFileName);

            string currentContent = File.ReadAllText(scriptPath);

            // If canonical version doesn't exist yet, create it
            if (!File.Exists(canonicalPath))
            {
                File.WriteAllText(canonicalPath, currentContent);
                continue;
            }

            string canonicalContent = File.ReadAllText(canonicalPath);
            float similarity = CalculateSimilarity(currentContent, canonicalContent);

            if (similarity < similarityThreshold)
            {
                // Save overridden version as txt with timestamp
                string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string overrideFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timeStamp}.txt";
                string overridePath = Path.Combine(overrideFolder, overrideFileName);

                File.WriteAllText(overridePath, currentContent);

                Debug.LogWarning($"[ScriptOverrideGuard] {fileName} was drastically changed (>90%). Saved overridden version to {overridePath}.");
                continue; // Do NOT update canonical version
            }

            // Minor change: update canonical version
            File.WriteAllText(canonicalPath, currentContent);
        }

        Debug.Log("ScriptOverrideGuard: All scripts checked.");
    }

    static float CalculateSimilarity(string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0f;

        int match = 0;
        int len = Mathf.Min(a.Length, b.Length);

        for (int i = 0; i < len; i++)
            if (a[i] == b[i]) match++;

        return (float)match / Mathf.Max(a.Length, b.Length);
    }

    [MenuItem("Tools/ScriptOverrideGuard/Enable")]
    public static void Enable()
    {
        IsEnabled = true;
        Debug.Log("ScriptOverrideGuard enabled.");
    }

    [MenuItem("Tools/ScriptOverrideGuard/Disable")]
    public static void Disable()
    {
        IsEnabled = false;
        Debug.Log("ScriptOverrideGuard disabled.");
    }

    [MenuItem("Tools/ScriptOverrideGuard/Enable", validate = true)]
    public static bool ValidateEnable()
    {
        return !IsEnabled;
    }

    [MenuItem("Tools/ScriptOverrideGuard/Disable", validate = true)]
    public static bool ValidateDisable()
    {
        return IsEnabled;
    }
}

