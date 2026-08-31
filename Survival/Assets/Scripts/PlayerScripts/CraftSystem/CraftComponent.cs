[System.Serializable]
public class CraftComponent
{
    public Item _item;
    public int _amount;

    public CraftComponent(Item item, int amount)
    {
        _item = item;
        _amount = amount;
    }
}
