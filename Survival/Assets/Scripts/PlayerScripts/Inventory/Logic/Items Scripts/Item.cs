using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected GameObject inHandPrefab;
    [SerializeField] protected bool isStackable = true;
    [SerializeField] protected int maxStackSize = 1;

    public string ID => id;
    public Sprite Icon => icon;
    public string ItemName => itemName;
    public string Description => description;
    public GameObject Prefab => prefab;
    public GameObject InHandPrefab => inHandPrefab;
    public bool IsStackable => isStackable;
    public int MaxStackSize => maxStackSize;
}
