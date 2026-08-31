using UnityEngine;

public class InventoryInputHandler : MonoBehaviour
{
    private Inventory inventory;
    private Animator animator;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (InputService.MouseScrollY != 0)
        {
            inventory.SlotSelect(-(int)InputService.MouseScrollY);
        }
        if (InputService.ItemDropPressed)
            inventory.DropItem(inventory.SelectedSlot);

        if (InputService._LMBPressed)
        {
            bool isEmptyState = animator.GetCurrentAnimatorStateInfo(0).IsName("empty state");
            if (inventory.SelectedSlot.ItemInSlot is IUsable usable && isEmptyState)
            {
                usable.Use(inventory.SelectedSlot, inventory);

                if (usable.UseAnimName != null)
                {
                    animator.Play(usable.UseAnimName);
                }
            }
        }
    }
}
