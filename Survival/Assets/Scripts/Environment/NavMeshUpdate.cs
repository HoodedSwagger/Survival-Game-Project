using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshUpdate : MonoBehaviour
{
    private NavMeshSurface surface;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        surface.UpdateNavMesh(surface.navMeshData);
    }

    private void OnEnable()
    {
        EventBus<NavMeshUpdateEvent>.Subscribe(UpdateNavMesh);
    }
    private void OnDisable()
    {
        EventBus<NavMeshUpdateEvent>.Unsubscribe(UpdateNavMesh);
    }
    private void UpdateNavMesh(NavMeshUpdateEvent evt)
    {
        surface.UpdateNavMesh(surface.navMeshData);
    }
}

