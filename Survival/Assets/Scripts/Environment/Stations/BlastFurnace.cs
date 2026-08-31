using System.Collections;
using UnityEngine;

public class BlastFurnace : MonoBehaviour,IInteractable, IPlaceable
{
    public string InteractText { get; set; }
    [SerializeField] private Transform outputSpawn;
    [SerializeField] private Item copperBar, tinBar, ironBar, coal;
    [SerializeField] private Item steel, bronze;

    [SerializeField] private float smeltingTime = 60f;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject lighting;
    [SerializeField] private AudioSource smeltSource;
    private bool isSmelting = false;

    public bool isInteractable = false;

    private void Start()
    {
        lighting.SetActive(false);
        InteractText = "to make bronze/steel";
    }
    public void Interact(GameObject interactor)
    {
        if (interactor == null || isSmelting || isInteractable == false) return;

        if (interactor.TryGetComponent(out Inventory inventory))
        {
            Debug.Log(inventory.name);
            if (inventory.GetTotalItemCount(ironBar) > 0 && inventory.GetTotalItemCount(coal) > 0)
            {
                inventory.RemoveItemsOfType(ironBar, 1);
                inventory.RemoveItemsOfType(coal, 5);
                StartCoroutine(SmeltingProcess(steel, 1));
            }
            else
                Debug.LogError($"Cant produce steel: iron - {inventory.GetTotalItemCount(ironBar)}, coal: {inventory.GetTotalItemCount(coal)}");
            if (inventory.GetTotalItemCount(copperBar) > 0 && inventory.GetTotalItemCount(tinBar) > 0)
            {
                //inventory.RemoveItemsOfType(coal, 5);
                inventory.RemoveItemsOfType(tinBar, 1);
                inventory.RemoveItemsOfType(copperBar, 1);
                StartCoroutine(SmeltingProcess(bronze, 2));
            }
            else
                Debug.LogError($"Cant produce bronze: tin - {inventory.GetTotalItemCount(tinBar)}, copper: {inventory.GetTotalItemCount(copperBar)}");
        }
        else
        {
            Debug.LogError("cant find inventory");
        }
    }
    public void Activate()
    {
        GetComponent<Collider>().enabled = true;
        isInteractable = true;
    }

    private IEnumerator SmeltingProcess(Item outputItem, int itemAmount)
    {
        isSmelting = true;
        particles.Play();
        lighting.SetActive(true);
        smeltSource.Play();

        yield return new WaitForSeconds(smeltingTime);

        for (int i = 0; i < itemAmount; i++)
        {
            Instantiate(outputItem.Prefab, outputSpawn.position, Quaternion.identity);
        }

        isSmelting = false;
        particles.Stop();
        lighting.SetActive(false);
        smeltSource.Stop();
    }
}
