using UnityEngine;
[CreateAssetMenu(fileName = "New Placeable", menuName = "Placeables/Create New Placeable")]
public class PlaceableItem : Item
{
    [SerializeField] private GameObject objectToPlace;
    public GameObject ObjectToPlace => objectToPlace;
}
