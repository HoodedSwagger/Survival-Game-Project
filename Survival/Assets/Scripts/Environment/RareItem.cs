using UnityEngine;

public class RareItem : MonoBehaviour
{
    private Vector3 spawnPosition;

    public void Init(Vector3 pos)
    {
        spawnPosition = new Vector3(pos.x, 0, pos.z);
    }

    public void OnDestroy()
    {
        EventBus<RareItemPickedEvent>.Raise(new RareItemPickedEvent { position = spawnPosition });
    }
}
