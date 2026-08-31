using UnityEngine;
public class InGameItem : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;
    [SerializeField] private int itemsCount = 1;
    public Item Item => item;
    public int ItemsCount => itemsCount;
    public string InteractText { get; set; }

    private void Start()
    {
        if (itemsCount > Item.MaxStackSize)
            itemsCount = Item.MaxStackSize;
        InteractText = $"to pick up x{itemsCount} {item.name}";
    }
    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent(out Inventory inventory))
        {
            int addedItemsCount = inventory.GetAddedCount(Item, ItemsCount);

            itemsCount -= addedItemsCount;

            InteractText = $"to pick up x{itemsCount} {item.name}";
            if (itemsCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
