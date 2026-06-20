using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CharacterVisualManager : MonoBehaviour
{
    [System.Serializable]
    private class prefabMount
    {
        public string variantName;
        public Transform mountTransform;
        public PrefabVariantGroupSO variantGroupSO;
    }

    [SerializeField]
    private List<prefabMount> prefabMounts = new List<prefabMount>();

    // runtime state: current instantiated visuals per slot
    private Dictionary<string, GameObject> currentInstances = new Dictionary<string, GameObject>();

    private void Awake()
    {
        LoadAndApplyAllSlots();
    }

    private void LoadAndApplyAllSlots()
    {
        foreach (var slot in prefabMounts)
        {
            string variantId = GameReference.PrefabVariantDict[slot.variantName];
            ApplyVariant(slot.variantName, variantId);
        }
    }

    public void ApplyVariant(string slotName, string variantId)
    {
        // find the slot mount
        prefabMount slot = prefabMounts.Find(s => s.variantName == slotName);
        if (slot == null)
        {
            Debug.LogWarning($"CharacterVisualManager: slot \"{slotName}\" not found on {gameObject.name}");
            return;
        }

        // find the variant definition in the ScriptableObject
        GameObject variant = slot.variantGroupSO.GetVariantById(variantId);


        // remove old instance if any
        if (currentInstances.TryGetValue(slotName, out GameObject old))
        {
            Destroy(old);
            currentInstances.Remove(slotName);
        }

        // instantiate new prefab as child of mount
        if (variant != null)
        {
            GameObject instance = Instantiate(variant, slot.mountTransform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            currentInstances[slotName] = instance;
        }
        else
        {
            Debug.LogWarning($"CharacterVisualManager: prefab for variant \"{variant.name}\" is null");
        }
    }
}
