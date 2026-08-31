using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory targetInventory;
    [SerializeField] private List<InventoryUISlot> _UISlots;
    [SerializeField] private RectTransform pointerRectTransform;
    private List<RectTransform> slotsRectTransform;
    private void OnEnable()
    {
        if (targetInventory == null)
        {
            return;
        }
        EventBus<SlotInfoChangeEvent>.Subscribe(UpdateSlot);
        EventBus<SlotSelectedEvent>.Subscribe(PointerUpdate);
    }

    private void OnDisable()
    {
        if (targetInventory != null)
        {
            EventBus<SlotInfoChangeEvent>.Unsubscribe(UpdateSlot);
            EventBus<SlotSelectedEvent>.Unsubscribe(PointerUpdate);
        }
    }
    private void Awake()
    {
        slotsRectTransform = new List<RectTransform>(_UISlots.Count);
        for (int i = 0; i < _UISlots.Count; i++)
        {
            slotsRectTransform.Add(_UISlots[i].GetComponent<RectTransform>());
        }
    }
    private void UpdateSlot(SlotInfoChangeEvent evt)
    {
        if (evt.Slot.ItemsCount == 0)
            _UISlots[evt.Slot.SlotIndex].CountText.SetText("");
        else
            _UISlots[evt.Slot.SlotIndex].CountText.SetText(evt.Slot.ItemsCount.ToString());

        if (evt.Slot.ItemInSlot == null)
            _UISlots[evt.Slot.SlotIndex].ItemIcon.sprite = null;
        else
            _UISlots[evt.Slot.SlotIndex].ItemIcon.sprite = evt.Slot.ItemInSlot.Icon;
    }

    private void PointerUpdate(SlotSelectedEvent evt)
    {
        pointerRectTransform.position = slotsRectTransform[evt.InventorySlot.SlotIndex].position;
    }
}
