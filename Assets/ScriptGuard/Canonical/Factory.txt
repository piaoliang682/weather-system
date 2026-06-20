using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : MonoBehaviour
{
    [Header("Trigger Zones")]
    public Transform outputLocation;    // Must be a trigger collider

    [Header("Recipes")]
    public List<RecipeDefinition> recipes;  // List of ScriptableObject recipes

    [Header("Effects")]
    public GameObject effectChild;          // Child effect to enable

    public ProgressBar progressBar;
    private bool isProducing = false;

    void Start()
    {
        if (effectChild != null)
            effectChild.SetActive(false);

        if (progressBar != null)
            progressBar.SetImageProgress(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure it's not currently producing
        if (isProducing) return;
            Debug.Log(other.name);
        ManufactureItem item=other.GetComponent<ManufactureItem>();
        if (item != null)
        {
            StartCoroutine(Produce(item.gameObject, GetRecipeForInput(item.itemID)));
        }
    }

    RecipeDefinition GetRecipeForInput(string inputTag)
    {
        Debug.Log("finding"+inputTag);
        return recipes.Find(r => r.inputID == inputTag);
    }

    IEnumerator Produce(GameObject rawObj, RecipeDefinition recipe)
    {
        isProducing = true;

        if (effectChild != null)
            effectChild.SetActive(true);

        rawObj.SetActive(false);

        float timer = 0f;
        float duration = recipe.productionTime;

        // Reset progress
        if (progressBar != null)
            progressBar.SetImageProgress(0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (progressBar != null)
                progressBar.SetImageProgress(timer / duration);

            yield return null;
        }

        // Ensure full
        if (progressBar != null)
            progressBar.SetImageProgress(1f);

        // Spawn product
        if (recipe.productPrefab != null)
        {
            Instantiate(recipe.productPrefab, outputLocation.position, Quaternion.identity);
        }

        if (effectChild != null)
            effectChild.SetActive(false);

        isProducing = false;
    }
}
