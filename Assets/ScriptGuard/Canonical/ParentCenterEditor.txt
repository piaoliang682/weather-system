using UnityEngine;

public class ParentCenterHelper : MonoBehaviour
{
    [ContextMenu("Center Parent To Children")]
    public void CenterParentToChildren()
    {
        Transform parent = transform;

        if (parent.childCount == 0)
        {
            Debug.LogWarning("[ParentCenterHelper] No children found");
            return;
        }

        // 1. Calculate center in world space
        Vector3 center = Vector3.zero;
        int count = 0;

        foreach (Transform child in parent)
        {
            center += child.position;
            count++;
        }

        center /= count;

        // 2. Store original parent position
        Vector3 parentPos = parent.position;

        // 3. Move parent to center
        parent.position = center;

        // 4. Compensate children so world position stays unchanged
        foreach (Transform child in parent)
        {
            child.position = child.position + (parentPos - center);
        }

        Debug.Log("[ParentCenterHelper] Parent centered without moving children");
    }
}