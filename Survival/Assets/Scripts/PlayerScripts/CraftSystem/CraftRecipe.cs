using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "New Recipe", menuName = "Craft/Add Recipe")]
public class CraftRecipe : ScriptableObject
{
    //[SerializeField] protected List<Item> itemsInRecipe = new List<Item>();
    //[SerializeField] protected List<int> itemsNeeded = new List<int>();
    [SerializeField] private List<CraftComponent> components = new List<CraftComponent>();

    [SerializeField] private Item itemToCraft;
    [SerializeField] private int craftAmount;

    public List<CraftComponent> Components => components;
    public Item ItemToCraft => itemToCraft;
    public int CraftAmount => craftAmount;
}
