using System.Collections.Generic;
using UnityEngine;

public class WorldStateManage : MonoBehaviour
{
    public static HashSet<Vector2> collectedResources = new HashSet<Vector2>();
    public static Vector2 RoundPos(Vector3 pos) => new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.z));

    public void AddToSet(Vector3 position)
    {
        collectedResources.Add(RoundPos(position));
    }
}
