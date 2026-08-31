using System.Collections;
using UnityEngine;

public class Harvestable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject itemToHarvest;
    [SerializeField] private GameObject stocked, harvested;

    [SerializeField] private Transform itemsSpawnPoint;
    [SerializeField] private int harvestAmount;
    [SerializeField] private float restockTime = 300f;

    private bool canHarvest = true;

    public string InteractText { get; set; }
    private void Start()
    {
        if (itemsSpawnPoint == null)
        {
            itemsSpawnPoint = transform;
        }
        InteractText = $"to harvest";
    }
    public void Interact(GameObject interactor)
    {
        if (!canHarvest) return;
        if (stocked != null)
            stocked.SetActive(false);
        if (harvested != null)
            harvested.SetActive(true);
        for (int i = 0; i < harvestAmount; i++)
        {
            Vector3 spawnPoint = itemsSpawnPoint.position + Random.insideUnitSphere;
            GameObject spawnedObject = Instantiate(itemToHarvest, spawnPoint, Quaternion.identity);
        }
        canHarvest = false;


        StartCoroutine(Restock());
    }

    private IEnumerator Restock()
    {
        yield return new WaitForSeconds(restockTime);

        canHarvest = true;
        if (stocked != null)
            stocked.SetActive(true);
        if (harvested != null)
            harvested.SetActive(false);
    }
}
