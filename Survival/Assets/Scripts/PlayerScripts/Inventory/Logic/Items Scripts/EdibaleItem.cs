using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Create New Ediable Item")]
public class EdibaleItem : Item, IUsable
{
    [SerializeField] private int saturation;
    [SerializeField] private int healthRestore;
    [SerializeField] private string useAnimName;
    public string UseAnimName { get => useAnimName; set => useAnimName = value; }

    public void Use(InventorySlot slot, Inventory invenotory)
    {
        FoodEatenEvent evt = new FoodEatenEvent()
        {
            _hungerRestoreAmount = saturation,
            _healthRestoreAmount = healthRestore,
        };
        EventBus<FoodEatenEvent>.Raise(evt);
       

        invenotory.RemoveItem(1, slot);
    }
}
