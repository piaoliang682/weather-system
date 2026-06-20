using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class RecenterMeshPivot : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Recenter Pivot")]
#endif
    public void RecenterPivot()
    {
#if UNITY_EDITOR
        MeshFilter mf = GetComponent<MeshFilter>();

        if (mf == null || mf.sharedMesh == null)
        {
            Debug.LogWarning("No mesh found.");
            return;
        }

        Mesh sourceMesh = mf.sharedMesh;

        Mesh mesh = Instantiate(sourceMesh);

        Vector3 center = mesh.bounds.center;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
            vertices[i] -= center;

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        string sourcePath = UnityEditor.AssetDatabase.GetAssetPath(sourceMesh);
        string folder = System.IO.Path.GetDirectoryName(sourcePath);
        string assetPath = $"{folder}/{sourceMesh.name}_PivotCentered.asset";

        Mesh existing =
            UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

        if (existing != null)
        {
            bool overwrite = UnityEditor.EditorUtility.DisplayDialog(
                "Overwrite Mesh?",
                $"'{sourceMesh.name}_PivotCentered.asset' already exists.\nOverwrite it?",
                "Overwrite",
                "Cancel"
            );

            if (!overwrite)
            {
                DestroyImmediate(mesh);
                return;
            }

            UnityEditor.EditorUtility.CopySerialized(mesh, existing);
            UnityEditor.EditorUtility.SetDirty(existing);
            UnityEditor.AssetDatabase.SaveAssets();

            DestroyImmediate(mesh);
            mesh = existing;
        }
        else
        {
            UnityEditor.AssetDatabase.CreateAsset(mesh, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        mf.sharedMesh = mesh;
        transform.position += transform.TransformVector(center);

        Debug.Log($"Pivot recentered for '{name}'");
#endif
    }
}