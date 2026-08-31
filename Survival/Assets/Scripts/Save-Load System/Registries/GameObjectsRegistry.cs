using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "GameObjectsRegistry", menuName = "Scriptable Objects/Registries/GameObjectsRegistry")]
public class GameObjectsRegistry : ScriptableObject
{
    [SerializeField] private List<GameObject> objectsRegistry = new List<GameObject>();

    public List<GameObject> ObjectsRegistry => objectsRegistry;

    public GameObject GetByType(string type)
    {
        return objectsRegistry.Find(obj => obj.GetComponent<Building>().ID == type);
    }
}
