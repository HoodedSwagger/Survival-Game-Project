using UnityEngine;

public interface IInteractable
{
    public string InteractText { get; set; }
    public void Interact(GameObject interactor);
}
