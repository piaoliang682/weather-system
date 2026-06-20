using UnityEngine;

public class RandomObjectSelector : MonoBehaviour
{
    [Header("Objects To Choose From")]
    public GameObject[] objects;

    [Header("Options")]
    public bool selectOnStart = true;
    public bool disableOthers = true;
    public bool allowNoneActive = false;

    [Header("Timing")]
    public bool useDelay = false;
    public float delay = 0f;

    void Start()
    {
        if (selectOnStart)
        {
            if (useDelay)
                Invoke(nameof(SelectRandom), delay);
            else
                SelectRandom();
        }
    }

    public void SelectRandom()
    {
        if (objects == null || objects.Length == 0)
        {
            Debug.LogWarning("No objects assigned.");
            return;
        }

        if (allowNoneActive && Random.value < 0.1f)
        {
            DisableAll();
            return;
        }

        int index = Random.Range(0, objects.Length);

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] == null) continue;

            bool isActive = (i == index);

            if (disableOthers)
                objects[i].SetActive(isActive);
            else if (isActive)
                objects[i].SetActive(true);
        }

        Debug.Log("Selected: " + objects[index].name);
    }

    public void DisableAll()
    {
        foreach (var obj in objects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void Reselect()
    {
        SelectRandom();
    }
}