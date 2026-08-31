using UnityEngine;

public class LifeFruit : MonoBehaviour, IInteractable
{
    [SerializeField] private int hpModifier = 0;
    public string InteractText { get; set; } = "Eat strange fruit";
    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent(out PlayerHealth health))
        {
            health.ModifyMaxHeath(hpModifier);
        }
        Destroy(gameObject);
    }
}
