using UnityEngine;

public class GolemStepsParticles : MonoBehaviour
{
    [SerializeField] private AudioSourceController controller;
    [SerializeField] private GameObject effect;
    [SerializeField] private LayerMask layer;
    private bool canSpawn = false;
    private void Update()
    {

        if (Physics.Raycast(transform.position, -transform.up, 0.2f, layer))
        {
            if (canSpawn)
            {
                canSpawn = false;
                Instantiate(effect, transform.position, Quaternion.identity);
                controller.Play();
            }
        }
        else
        {
            canSpawn = true;
        }
    }
}
