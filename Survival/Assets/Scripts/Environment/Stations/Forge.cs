using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Forge : MonoBehaviour, IInteractable, IPlaceable
{
    [SerializeField] private List<Item> ores;
    [SerializeField] private Transform outputSpawn;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject lighting;
    [SerializeField] private AudioSource smeltSource;

    public bool isInteractable = false;
    private bool isSmelting = false;
    private ResourceItem smeltingItem;

    [SerializeField] private float smeltingTime = 20f;

    private float timer = 0;

    public string InteractText { get; set; }

    private void Start()
    {
        lighting.SetActive(false);
        InteractText = "to smelt ore";
    }
    public void Interact(GameObject interactor)
    {
        if (interactor == null || isSmelting || isInteractable == false) return;

        if (interactor.TryGetComponent(out Inventory inventory))
        {
            InventorySlot slot = inventory.SelectedSlot;

            if (slot == null) return;
            if (!ores.Contains(slot.ItemInSlot)) return;
            smeltingItem = slot.ItemInSlot as ResourceItem;
            inventory.RemoveItem(1, slot);
            StartCoroutine(SmeltingProcess());
        }
    }
    public void Activate()
    {
        GetComponent<Collider>().enabled = true;
        isInteractable = true;
    }

    private IEnumerator SmeltingProcess()
    {
        isSmelting = true;
        particles.Play();
        lighting.SetActive(true);
        smeltSource.Play();

        yield return new WaitForSeconds(smeltingTime);

        GameObject spawnedObject = Instantiate(smeltingItem.ProcessedResourceGameObject, outputSpawn.position, Quaternion.identity);

        lighting.SetActive(false);
        particles.Stop();
        smeltSource.Stop();
        isSmelting = false;
    }
}
