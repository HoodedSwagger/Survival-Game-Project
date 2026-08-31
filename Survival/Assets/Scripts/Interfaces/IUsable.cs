public interface IUsable
{
    public string UseAnimName { get; set; }
    public void Use(InventorySlot slot, Inventory inventory);
}
