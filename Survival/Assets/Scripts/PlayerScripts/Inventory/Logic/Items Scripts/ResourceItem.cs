using UnityEngine;

[CreateAssetMenu(fileName = "ResourceItem", menuName = "Item/ResourceItem")]
public class ResourceItem : Item
{
    [SerializeField] private GameObject processedResource;

    public GameObject ProcessedResourceGameObject => processedResource;
}
