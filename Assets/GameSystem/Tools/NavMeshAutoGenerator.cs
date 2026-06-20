using UnityEngine;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshAutoGenerator : MonoBehaviour
{
    private NavMeshSurface surface;

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();

        Debug.Log("Generating NavMesh...");
        surface.BuildNavMesh();   // 🧠 Automatically builds navmesh at runtime
        Debug.Log("NavMesh generated!");
    }
}
