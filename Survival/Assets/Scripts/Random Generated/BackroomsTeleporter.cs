using UnityEngine;

public class BackroomsTeleporter : MonoBehaviour
{
    public Transform backroomsSpawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.TryGetComponent(out CharacterController controller))
        {
            controller.enabled = false;
            other.transform.position = backroomsSpawnPoint.position;
            controller.enabled = true;
        }
    }
}
