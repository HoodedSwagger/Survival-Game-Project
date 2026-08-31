using UnityEngine;

public class ResourceSource : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] private int dropAmount = 1;
    [SerializeField] private int maxDurabilty = 1;
    private int currentDurability;

    private Animator animator;
    [SerializeField] private string animName;

    private void Start()
    {
        currentDurability = maxDurabilty;

        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int toolDamage)
    {
        currentDurability -= toolDamage;
        if (currentDurability <= 0)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                GameObject spawnedItem = Instantiate(itemToDrop);

                Vector3 offset = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0, 0.5f),
                    Random.Range(-0.5f, 0.5f)
                    );
                spawnedItem.transform.position = transform.position + offset;
            }

            if(animator != null)
                animator.Play(animName);
            else
            {
                GetDestroy();
            }
                dropAmount = 0;
        }
    }
    public void GetDestroy()
    {
        EventBus<ResourceHarvestedEvent>.Raise(new ResourceHarvestedEvent {position = transform.position});
        Destroy(gameObject);
    }
}
