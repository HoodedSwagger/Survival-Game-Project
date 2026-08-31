using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Item item;
    public string ID => item.ID;
}
