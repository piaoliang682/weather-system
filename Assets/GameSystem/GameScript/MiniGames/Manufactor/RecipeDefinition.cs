using UnityEngine;

[CreateAssetMenu(fileName = "InputToProduct", menuName = "Game/Manufacture/Recipe")]
public class RecipeDefinition : ScriptableObject
{
    public string inputID;        // Example: "IronOre", "Wood"
    public float productionTime;
    public GameObject productPrefab;
}
