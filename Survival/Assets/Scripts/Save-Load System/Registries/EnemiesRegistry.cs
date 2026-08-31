using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "EnemiesRegistry", menuName = "Scriptable Objects/Registries/EnemiesRegistry")]
public class EnemiesRegistry : ScriptableObject
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> Enemies => enemies;

    public GameObject GetByID(string id)
    {
        return enemies.Find(enemy => enemy.GetComponent<Enemy>().ID == id);
    }
}
