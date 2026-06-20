using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class VisualContainer : MonoBehaviour
{
    [System.Serializable]
    public class VisualCategory
    {
        public string categoryName;
        public GameObject holder;
        public VisualEntry[] visualList;
    }

    [System.Serializable]
    public class VisualEntry
    {
        public string id;        // unique ID
        public GameObject obj;   // actual child gameobject
    }

    public VisualCategory[] categories;
    public bool setDefault;
    private Dictionary<string, VisualCategory> categoryMap;

    private void Awake()
    {
        Debug.Log("Setup"+gameObject.name);
        categoryMap = new Dictionary<string, VisualCategory>();
        foreach (var cat in categories)
        {
            if (!string.IsNullOrEmpty(cat.categoryName))
                categoryMap[cat.categoryName] = cat;
        }
    }

    private void OnValidate()
    {
        foreach (var cat in categories)
            FillFromHolder(cat);
    }


    private void Start()
    {
        // Optional: enable the first visual only, disable the rest
        if (setDefault)
        {
            foreach (var cat in categories)
            {
                if (cat.visualList != null && cat.visualList.Length > 0)
                    SetActiveVisual(cat, 0);
            }
        }

    }

    private void FillFromHolder(VisualCategory cat)
    {
        if (cat.holder == null)
        {
            Debug.LogWarning($"No holder assigned for {cat.categoryName}");
            cat.visualList = new VisualEntry[0];
            return;
        }

        int childCount = cat.holder.transform.childCount;
        cat.visualList = new VisualEntry[childCount];

        for (int i = 0; i < childCount; i++)
        {
            var child = cat.holder.transform.GetChild(i).gameObject;
            var entry = new VisualEntry
            {
                id = child.name,
                obj = child
            };
            cat.visualList[i] = entry;
        }

    }

    /// <summary>
    /// Set specific visual by ID under a category. Returns true if successful.
    /// </summary>
    public bool SetVisualByID(string category, string id)
    {
        var cat = GetCategory(category);
        if (cat == null) return false;

        bool found = false;
        bool noneMode = string.IsNullOrEmpty(id) || id.ToLower() == "none";

        for (int i = 0; i < cat.visualList.Length; i++)
        {
            var entry = cat.visualList[i];
            if (entry.obj == null) continue;

            bool shouldEnable = !noneMode && entry.id == id;
            entry.obj.SetActive(shouldEnable);
            //Debug.Log($"{entry.obj.name} is {shouldEnable}");
            if (shouldEnable)
                found = true;
        }

        if (!found && !noneMode)
            Debug.LogWarning($"Visual ID '{id}' not found in category '{category}'!");

        return found || noneMode;
    }

    /// <summary>
    /// Cycle to the next visual in a category.
    /// </summary>
    public void CycleVisual(string category)
    {
        var cat = GetCategory(category);
        if (cat == null) return;

        int activeIndex = -1;
        for (int i = 0; i < cat.visualList.Length; i++)
        {
            if (cat.visualList[i].obj != null && cat.visualList[i].obj.activeSelf)
            {
                activeIndex = i;
                break;
            }
        }

        int nextIndex = (activeIndex + 1) % cat.visualList.Length;
        SetActiveVisual(cat, nextIndex);
    }

    private void SetActiveVisual(VisualCategory cat, int indexToEnable)
    {
        for (int i = 0; i < cat.visualList.Length; i++)
        {
            if (cat.visualList[i].obj != null)
                cat.visualList[i].obj.SetActive(i == indexToEnable);
        }
    }

    private VisualCategory GetCategory(string categoryName)
    {
        if (categoryMap.TryGetValue(categoryName, out var cat))
            return cat;

        Debug.LogWarning($"Category '{categoryName}' not found!");
        return null;
    }

    /// <summary>
    /// Get all visual IDs in a category (for UI lists/buttons).
    /// </summary>
    public string[] GetIDsInCategory(string category)
    {
        var cat = GetCategory(category);
        if (cat == null) return new string[0];
        return cat.visualList.Select(v => v.id).ToArray();
    }
}
