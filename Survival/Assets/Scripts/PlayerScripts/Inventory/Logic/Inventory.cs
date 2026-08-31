using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slotsCount = 1;
    public int SlotsCount => slotsCount;

    private InventorySlot selectedSlot;

    private List<InventorySlot> slots;

    private Animator animator;

    public InventorySlot SelectedSlot => selectedSlot;

    private void Awake()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < slotsCount; i++)
        {
            slots.Add(new InventorySlot(null, 0, i));
        }
        selectedSlot = slots[0];

        animator = GetComponent<Animator>();
    }
    public int GetAddedCount(Item item, int amount)
    {
        int leftToAdd = amount;
        int addedAmount = 0;
        List<InventorySlot> emptySlots = new List<InventorySlot>();
        foreach (var slot in slots)
        {
            if (slot.ItemInSlot == null)
            {
                emptySlots.Add(slot);
            }
            if (slot.ItemInSlot == item)
            {
                int canAddAmount = item.MaxStackSize - slot.ItemsCount;

                if (leftToAdd > canAddAmount)
                {
                    slot.AddItems(item, canAddAmount);
                    leftToAdd -= canAddAmount;
                    addedAmount += canAddAmount;
                }
                else
                {
                    slot.AddItems(item, leftToAdd);
                    addedAmount += leftToAdd;
                    leftToAdd = 0;

                    RefreshCurrentSlot();
                    return addedAmount;
                }
            }
        }
        if (leftToAdd > 0)
        {
            for (int i = 0; i < emptySlots.Count; i++)
            {
                int canAddAmount = item.MaxStackSize;

                if (leftToAdd > canAddAmount)
                {
                    emptySlots[i].AddItems(item, canAddAmount);
                    leftToAdd -= canAddAmount;
                    addedAmount += canAddAmount;
                }
                else
                {
                    emptySlots[i].AddItems(item, leftToAdd);
                    addedAmount += leftToAdd;
                    leftToAdd = 0;

                    RefreshCurrentSlot();
                    return addedAmount;
                }
            }
        }
        return addedAmount;
    }
    public void RemoveItem(int amount, InventorySlot slot)
    {
        if (slot.ItemInSlot == null)
        {
            Debug.LogWarning($"Slot {slot.SlotIndex} is empty");
            return;
        }

        slot.RemoveItems(amount);

        RefreshCurrentSlot();

    }
    public void RemoveItemsOfType(Item item, int amount)
    {
        foreach (var slot in slots)
        {
            if (amount <= 0) break;
            if (slot.ItemInSlot != item) continue;

            int toRemove = Mathf.Min(slot.ItemsCount, amount);
            slot.RemoveItems(toRemove);
            amount -= toRemove;
        }
    }
    public void DropItem(InventorySlot slot)
    {
        if (slot.ItemInSlot != null && slot.ItemsCount > 0)
        {
            GameObject obj = Instantiate(slot.ItemInSlot.Prefab,
                    transform.position + transform.forward,
                    Quaternion.identity);
            slot.RemoveItems(1);

            RefreshCurrentSlot();
        }
    }

    public void SlotSelect(int scrollWheelInput)
    {
        int newSelectedSlotIndex = selectedSlot.SlotIndex + scrollWheelInput;
        if (newSelectedSlotIndex > slots.Count - 1)
        {
            newSelectedSlotIndex = 0;
        }
        if (newSelectedSlotIndex < 0)
        {
            newSelectedSlotIndex = slots.Count - 1;
        }
        selectedSlot = slots[newSelectedSlotIndex];

        SlotSelectedEvent slotSelected = new SlotSelectedEvent()
        {
            InventorySlot = selectedSlot
        };
        EventBus<SlotSelectedEvent>.Raise(slotSelected);

        ItemInHands item = new ItemInHands();

        if (selectedSlot.ItemInSlot != null)
        {
            item._Item = selectedSlot.ItemInSlot;
        }
        EventBus<ItemInHands>.Raise(item);
    }

    public List<InventorySlot> GetItemsInInventory()
    {
        return slots;
    }
    public List<InventorySlot> FindSlotsWithItem(Item item)
    {
        List<InventorySlot> slotsWithTheItem = new List<InventorySlot>();
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].ItemInSlot == item)
            {
                slotsWithTheItem.Add(slots[i]);
            }
        }
        return slotsWithTheItem;
    }
    public int GetTotalItemCount(Item item)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.ItemInSlot == item)
                total += slot.ItemsCount;
        }
        return total;
    }

    public int ItemIndex(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].ItemInSlot == item)
            {
                return i;
            }
        }
        return -1;
    }

    public void RefreshCurrentSlot()
    {
        ItemInHands item = new ItemInHands();

        item._Item = selectedSlot.ItemInSlot;

        EventBus<ItemInHands>.Raise(item);
    }

}