using UnityEngine;
using System.Collections.Generic;

public class VisualController : MonoBehaviour
{
    private Dictionary<string, string> selectedVisuals =
        new Dictionary<string, string>();

    [SerializeField]
    private List<VisualContainer> visualContainerList =
        new List<VisualContainer>();

    private void Awake()
    {

        // Automatically find all VisualContainer in children (and self)
        VisualContainer[] containers = GetComponentsInChildren<VisualContainer>(true);
        foreach (var container in containers)
            Register(container);
    }

    private void OnValidate()
    {
        // In editor, keep inspector list synced
        SyncListenersFromChildren();
    }

    private void SyncListenersFromChildren()
    {
        visualContainerList.Clear();
        VisualContainer[] containers = GetComponentsInChildren<VisualContainer>(true);
        foreach (var container in containers)
            visualContainerList.Add(container);
    }

    public void Register(VisualContainer container)
    {

        if (!visualContainerList.Contains(container))
            visualContainerList.Add(container);
    }

    public void Unregister(VisualContainer container)
    {
        visualContainerList.Remove(container);
    }

    public void SetVisual(string category, string id)
    {
        selectedVisuals[category] = id;
        NotifyListeners(category, id);
    }


    public List<string> GetVisualIDListFromCategory(string categoryName)
    {
        HashSet<string> ids = new HashSet<string>();

        for (int i = 0; i < visualContainerList.Count; i++)
        {
            var container = visualContainerList[i];
            if (container == null)
                continue;

            var containerIDs = container.GetIDsInCategory(categoryName);
            if (containerIDs == null)
                continue;

            for (int j = 0; j < containerIDs.Length; j++)
            {
                ids.Add(containerIDs[j]); // HashSet avoids duplicates
            }
        }

        return new List<string>(ids);
    }

    private void NotifyListeners(string category, string id)
    {
        for (int i = 0; i < visualContainerList.Count; i++)
        {
            var container = visualContainerList[i];
            if (container != null)
                container.SetVisualByID(category, id);
        }
    }
}
