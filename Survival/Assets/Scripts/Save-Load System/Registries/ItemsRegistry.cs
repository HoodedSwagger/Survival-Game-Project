using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsRegistry", menuName = "Scriptable Objects/Registries/ItemsRegistry")]
public class ItemsRegistry : ScriptableObject
{
    [SerializeField] private List<Item> items = new List<Item>();

    public List<Item> Items => items;

    public Item GetByID(string id)
    {
        return items.Find(item => item.ID == id);
    }
}
