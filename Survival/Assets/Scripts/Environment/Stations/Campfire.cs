using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Campfire : MonoBehaviour, IInteractable, IPlaceable
{
    [SerializeField] private GameObject cookedSteak;
    [SerializeField] private Item rawSteak;
    [SerializeField] private Transform outputSpawn;
    [SerializeField] private ParticleSystem particles;
    private bool isSmelting = false;

    [SerializeField] private float smeltingTime = 10f;

    private float timer = 0;

    public string InteractText { get; set; } = "to cook meat";

    public void Activate()
    {
        if (gameObject.TryGetComponent(out Collider collider))
        {
            collider.enabled = true;
        }
    }

    public void Interact(GameObject interactor)
    {
        if (interactor == null || isSmelting) return;

        if (interactor.TryGetComponent(out Inventory inventory))
        {
            int index = inventory.ItemIndex(rawSteak);
            if (index == -1) return;
            List<InventorySlot> slots = inventory.GetItemsInInventory();
            inventory.RemoveItem(1, slots[index]);
            StartCoroutine(SmeltingProcess());
        }
    }

    private IEnumerator SmeltingProcess()
    {
        isSmelting = true;
        if(particles != null) 
            particles.Play();
        yield return new WaitForSeconds(smeltingTime);

        GameObject spawnedObject = Instantiate(cookedSteak, outputSpawn.position, Quaternion.identity);

        if(particles != null) 
            particles.Stop();
        isSmelting = false;
    }
}
