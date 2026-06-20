using System.Collections.Generic;
using UnityEngine;

public class RigComparer : MonoBehaviour
{
    public Transform modelA;
    public Transform modelB;

    public bool logDetails = true;

    [ContextMenu("Deep Compare Rigs")]
    public void Compare()
    {
        if (!modelA || !modelB)
        {
            Debug.LogError("Assign both models first!");
            return;
        }

        Debug.Log("=== START RIG DEEP COMPARE ===");

        CompareTransforms(modelA, modelB);

        CompareSkinnedMeshes(modelA, modelB);

        Debug.Log("=== FINISHED ===");
    }

    // -------------------------------
    // 1. TRANSFORM CHECK (Bones)
    // -------------------------------
    void CompareTransforms(Transform a, Transform b)
    {
        if (a.name != b.name)
        {
            Debug.LogWarning($"Name mismatch: {a.name} vs {b.name}");
        }

        // Position check (local)
        if (!Approximately(a.localPosition, b.localPosition))
            Log($"Local Position mismatch: {a.name} with {b.name}");

        // Rotation check
        if (!Approximately(a.localRotation, b.localRotation))
            Log($"Local Rotation mismatch: {a.name} with {b.name}");

        // Scale check (VERY IMPORTANT)
        if (!Approximately(a.localScale, b.localScale))
            Log($"Local Scale mismatch: {a.name} with {b.name}");

        // Children
        if (a.childCount != b.childCount)
        {
            Debug.LogWarning($"Child count mismatch at {a.name}");
        }

        int count = Mathf.Min(a.childCount, b.childCount);

        for (int i = 0; i < count; i++)
        {
            CompareTransforms(a.GetChild(i), b.GetChild(i));
        }
    }

    // -------------------------------
    // 2. SKINNED MESH CHECK
    // -------------------------------
    void CompareSkinnedMeshes(Transform aRoot, Transform bRoot)
    {
        var aMeshes = aRoot.GetComponentsInChildren<SkinnedMeshRenderer>();
        var bMeshes = bRoot.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (aMeshes.Length != bMeshes.Length)
        {
            Debug.LogWarning($"SkinnedMesh count mismatch: {aMeshes.Length} vs {bMeshes.Length}");
        }

        int count = Mathf.Min(aMeshes.Length, bMeshes.Length);

        for (int i = 0; i < count; i++)
        {
            var a = aMeshes[i];
            var b = bMeshes[i];

            Debug.Log($"Checking mesh: {a.name}");

            // Bind pose count check
            if (a.sharedMesh.bindposes.Length != b.sharedMesh.bindposes.Length)
            {
                Debug.LogWarning($"Bindpose count mismatch in {a.name}");
            }

            // Bone count check
            if (a.bones.Length != b.bones.Length)
            {
                Debug.LogWarning($"Bone count mismatch in {a.name}");
            }

            // Root bone check
            if (a.rootBone != null && b.rootBone != null)
            {
                if (a.rootBone.name != b.rootBone.name)
                {
                    Debug.LogWarning($"Root bone mismatch in {a.name}");
                }
            }
        }
    }

    // -------------------------------
    // UTILITIES
    // -------------------------------
    bool Approximately(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) < 0.0001f;
    }

    bool Approximately(Quaternion a, Quaternion b)
    {
        return Quaternion.Angle(a, b) < 0.01f;
    }

    void Log(string msg)
    {
        if (logDetails)
            Debug.LogWarning(msg);
    }
}