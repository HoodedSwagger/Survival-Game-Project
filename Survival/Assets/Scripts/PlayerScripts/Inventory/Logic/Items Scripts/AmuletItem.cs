using UnityEngine;

[CreateAssetMenu(fileName = "AmuletItem", menuName = "Scriptable Objects/AmuletItem")]
public class AmuletItem : Item, IUsable
{
    [Range(0, 3)] [SerializeField] private int amuletIndex;
    [SerializeField] private int healthAdd;
    [SerializeField] private string useAnimName;
    public string UseAnimName { get => useAnimName; set => useAnimName = value; }

    public void Use(InventorySlot slot, Inventory invenotory)
    {
        EventBus<AmuletUsedEvent>.Raise(new AmuletUsedEvent { AmuletIndex = amuletIndex, increase = healthAdd });

        invenotory.RemoveItem(1, slot);
    }
}
