using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshDoor : MonoBehaviour
{
    public bool isLocked = true;
    public NavMeshSurface surface;

    public void UnlockDoor()
    {
        isLocked = false;
        // animação, desativar collider, etc.

        // Atualizar navegação
        surface.BuildNavMesh();
    }
}
