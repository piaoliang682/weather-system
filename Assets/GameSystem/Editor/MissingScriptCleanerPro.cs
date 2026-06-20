#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissingScriptCleanerPro : EditorWindow
{
    private Vector2 scroll;
    private readonly List<string> logs = new List<string>();

    [MenuItem("Tools/Missing Script Cleaner Pro")]
    public static void Open()
    {
        GetWindow<MissingScriptCleanerPro>("Missing Script Cleaner");
    }

    private void OnGUI()
    {
        GUILayout.Space(8);

        EditorGUILayout.HelpBox(
            "用于清理场景、选中物体、Prefab 源文件里的 Missing Script。\n" +
            "如果提示 Prefab file contains an invalid script，请优先清理 Prefab 源文件。",
            MessageType.Info
        );

        GUILayout.Space(8);

        if (GUILayout.Button("清理当前打开的所有场景", GUILayout.Height(34)))
        {
            ClearLogs();
            CleanAllOpenScenes();
        }

        GUILayout.Space(4);

        if (GUILayout.Button("清理选中对象 / Prefab 源文件", GUILayout.Height(34)))
        {
            ClearLogs();
            CleanSelectionSmart();
        }

        GUILayout.Space(4);

        if (GUILayout.Button("清理项目内所有 Prefab", GUILayout.Height(34)))
        {
            ClearLogs();

            bool ok = EditorUtility.DisplayDialog(
                "清理所有 Prefab",
                "即将扫描并修改项目内所有 Prefab。\n\n建议先提交 Git / 备份项目。\n\n是否继续？",
                "继续",
                "取消"
            );

            if (ok)
            {
                CleanAllPrefabsInProject();
            }
        }

        GUILayout.Space(8);

        if (GUILayout.Button("保存 Assets 和场景", GUILayout.Height(28)))
        {
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveOpenScenes();
            Log("已保存 Assets 和当前打开场景。");
        }

        GUILayout.Space(12);

        EditorGUILayout.LabelField("日志", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (string log in logs)
        {
            EditorGUILayout.LabelField(log, EditorStyles.wordWrappedLabel);
        }
        EditorGUILayout.EndScrollView();
    }

    private void ClearLogs()
    {
        logs.Clear();
    }

    private void Log(string message)
    {
        logs.Add(message);
        Debug.Log("[MissingScriptCleanerPro] " + message);
        Repaint();
    }

    // ===============================
    // 清理当前打开场景
    // ===============================

    private void CleanAllOpenScenes()
    {
        int totalRemoved = 0;
        int totalObjects = 0;

        HashSet<Scene> dirtyScenes = new HashSet<Scene>();

        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Clean Missing Scripts In Scenes");

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            GameObject[] roots = scene.GetRootGameObjects();

            foreach (GameObject root in roots)
            {
                CleanGameObjectHierarchy(root, true, ref totalRemoved, ref totalObjects);

                if (totalRemoved > 0)
                {
                    dirtyScenes.Add(scene);
                }
            }
        }

        foreach (Scene scene in dirtyScenes)
        {
            EditorSceneManager.MarkSceneDirty(scene);
        }

        Undo.CollapseUndoOperations(undoGroup);

        Log($"场景清理完成。移除 Missing Script：{totalRemoved} 个，影响物体：{totalObjects} 个。");
    }

    // ===============================
    // 智能清理选中
    // 选中场景实例时，会尝试清理对应 Prefab 源文件
    // 选中 Project 面板中的 Prefab 时，会直接清理 Prefab Asset
    // ===============================

    private void CleanSelectionSmart()
    {
        UnityEngine.Object[] selected = Selection.objects;

        if (selected == null || selected.Length == 0)
        {
            EditorUtility.DisplayDialog("提示", "请先选中场景物体或 Project 面板里的 Prefab。", "OK");
            return;
        }

        int totalRemoved = 0;
        int totalObjects = 0;
        int prefabCleaned = 0;

        HashSet<string> prefabPaths = new HashSet<string>();

        foreach (UnityEngine.Object obj in selected)
        {
            if (obj == null) continue;

            string assetPath = AssetDatabase.GetAssetPath(obj);

            // Project 面板里直接选中的 Prefab
            if (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                prefabPaths.Add(assetPath);
                continue;
            }

            GameObject go = obj as GameObject;
            if (go == null) continue;

            // 场景中的 Prefab 实例，找到它的源 Prefab
            string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            if (!string.IsNullOrEmpty(prefabPath))
            {
                prefabPaths.Add(prefabPath);
            }
            else
            {
                // 普通场景物体
                CleanGameObjectHierarchy(go, true, ref totalRemoved, ref totalObjects);

                if (go.scene.IsValid() && go.scene.isLoaded)
                {
                    EditorSceneManager.MarkSceneDirty(go.scene);
                }
            }
        }

        foreach (string path in prefabPaths)
        {
            int removed;
            int objects;

            bool changed = CleanPrefabAsset(path, out removed, out objects);

            if (changed)
            {
                prefabCleaned++;
                totalRemoved += removed;
                totalObjects += objects;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Log($"选中内容清理完成。清理 Prefab：{prefabCleaned} 个，移除 Missing Script：{totalRemoved} 个，影响物体：{totalObjects} 个。");
    }

    // ===============================
    // 清理项目所有 Prefab
    // ===============================

    private void CleanAllPrefabsInProject()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        int prefabCount = guids.Length;
        int changedPrefabCount = 0;
        int totalRemoved = 0;
        int totalObjects = 0;

        try
        {
            for (int i = 0; i < prefabCount; i++)
            {
                string guid = guids[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);

                EditorUtility.DisplayProgressBar(
                    "清理 Missing Script",
                    path,
                    prefabCount == 0 ? 1f : (float)i / prefabCount
                );

                int removed;
                int objects;

                bool changed = CleanPrefabAsset(path, out removed, out objects);

                if (changed)
                {
                    changedPrefabCount++;
                    totalRemoved += removed;
                    totalObjects += objects;
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Log($"项目 Prefab 清理完成。扫描 Prefab：{prefabCount} 个，修改 Prefab：{changedPrefabCount} 个，移除 Missing Script：{totalRemoved} 个，影响物体：{totalObjects} 个。");
    }

    // ===============================
    // 核心：清理 Prefab Asset 本体
    // ===============================

    private bool CleanPrefabAsset(string prefabPath, out int removedCount, out int affectedObjectCount)
    {
        removedCount = 0;
        affectedObjectCount = 0;

        if (string.IsNullOrEmpty(prefabPath))
            return false;

        if (!prefabPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            return false;

        GameObject prefabRoot = null;

        try
        {
            prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            if (prefabRoot == null)
            {
                Log($"无法加载 Prefab：{prefabPath}");
                return false;
            }

            CleanGameObjectHierarchy(prefabRoot, false, ref removedCount, ref affectedObjectCount);

            if (removedCount > 0)
            {
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
                Log($"已清理 Prefab：{prefabPath}，移除 {removedCount} 个 Missing Script。");
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            Log($"清理 Prefab 失败：{prefabPath}\n{e.Message}");
            return false;
        }
        finally
        {
            if (prefabRoot != null)
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }
    }

    // ===============================
    // 核心：清理一个 GameObject 层级
    // includeUndo = true 用于场景对象
    // includeUndo = false 用于 Prefab 临时加载对象
    // ===============================

    private void CleanGameObjectHierarchy(
        GameObject root,
        bool includeUndo,
        ref int removedCount,
        ref int affectedObjectCount
    )
    {
        if (root == null) return;

        Transform[] transforms = root.GetComponentsInChildren<Transform>(true);

        if (includeUndo)
        {
            Undo.RegisterFullObjectHierarchyUndo(root, "Remove Missing Scripts");
        }

        foreach (Transform t in transforms)
        {
            if (t == null) continue;

            GameObject go = t.gameObject;

            int missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

            if (missingCount <= 0)
                continue;

            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);

            if (removed > 0)
            {
                removedCount += removed;
                affectedObjectCount++;

                EditorUtility.SetDirty(go);

                Log($"移除 Missing Script：{GetGameObjectPath(go)}，数量：{removed}");
            }
        }
    }

    private string GetGameObjectPath(GameObject go)
    {
        if (go == null) return "Null";

        string path = go.name;
        Transform current = go.transform.parent;

        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }

        return path;
    }
}
#endif