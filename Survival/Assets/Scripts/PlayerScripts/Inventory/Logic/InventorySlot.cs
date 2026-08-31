public class InventorySlot
{
    private Item itemInSlot;
    private int itemsCount = 0;
    private int slotIndex;
    public Item ItemInSlot => itemInSlot;
    public int ItemsCount => itemsCount;
    public int SlotIndex => slotIndex;

    public InventorySlot(Item item, int count, int index)
    {
        itemInSlot = item;
        itemsCount = count;
        slotIndex = index;
    }

    public void AddItems(Item item, int amount)
    {
        if (amount == 0) return;

        if (itemInSlot == null)
        {
            itemInSlot = item;

            itemsCount += amount;
        }
        else
        {
            itemsCount += amount;
        }
        SendUpdateToUI();
    }
    public void RemoveItems(int amount)
    {
        if (itemInSlot == null) return;
        if (itemsCount >= amount)
        {
            itemsCount -= amount;
        }
        else
            return;
        if (itemsCount == 0)
        {
            itemInSlot = null;
        }
        SendUpdateToUI();
    }
    public void SendUpdateToUI()
    {
        EventBus<SlotInfoChangeEvent>.Raise(new SlotInfoChangeEvent()
        {
            Slot = this
        });
    }
}
