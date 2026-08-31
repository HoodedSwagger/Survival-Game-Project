using UnityEngine;
using System.Collections.Generic;
public class Craft : MonoBehaviour
{
    public Inventory playerInventory;

    private void OnEnable()
    {
        EventBus<ItemCraftedEvent>.Subscribe(Craft_Improved2);
    }
    private void OnDisable()
    {
        EventBus<ItemCraftedEvent>.Unsubscribe(Craft_Improved2);
    }
    private void Craft_Improved2(ItemCraftedEvent craftEvent)
    {
        List<CraftComponent> components = craftEvent.itemRecipe.Components;

        foreach (var component in components)
        {
            int total = playerInventory.GetTotalItemCount(component._item);
            if (component._amount > total) return;
        }
        foreach (var component in components)
        {
            playerInventory.RemoveItemsOfType(component._item, component._amount);
        }
        playerInventory.GetAddedCount(craftEvent.itemRecipe.ItemToCraft, craftEvent.itemRecipe.CraftAmount);
    }

    private void Craft_Improved(ItemCraftedEvent craftEvent)
    {
        List<InventorySlot> slots = playerInventory.GetItemsInInventory();

        List<CraftComponent> components = craftEvent.itemRecipe.Components;

        List<InventorySlot> suitableSlots = new List<InventorySlot>();
        List<int> neededAmount = new List<int>();

        foreach (var slot in slots)
        {
            foreach (var component in components)
            {
                if (component._item == slot.ItemInSlot && component._amount <= slot.ItemsCount)
                {
                    suitableSlots.Add(slot);
                    neededAmount.Add(component._amount);
                }
            }
        }
        if (suitableSlots.Count == components.Count)
        {
            for (int i = 0; i < suitableSlots.Count; i++)
            {
                suitableSlots[i].RemoveItems(neededAmount[i]);
            }
            playerInventory.GetAddedCount(craftEvent.itemRecipe.ItemToCraft, craftEvent.itemRecipe.CraftAmount);
        }
    }

}
