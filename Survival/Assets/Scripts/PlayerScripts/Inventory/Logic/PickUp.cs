using UnityEngine;
using TMPro;
public class PickUp : MonoBehaviour
{
    [SerializeField] private float rayLength;

    [SerializeField] private Transform rayOutput;

    [SerializeField] private LayerMask pickableLayerMask;

    [SerializeField] private TMP_Text text;
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOutput.position, rayOutput.forward, out hit, rayLength, pickableLayerMask))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                text.SetText($"'E' {interactable.InteractText}");
                if (!Input.GetKeyDown(KeyCode.E)) return;
                interactable.Interact(gameObject);
            }
        }
        else
        {
            text.SetText("");
        }
    }
}
