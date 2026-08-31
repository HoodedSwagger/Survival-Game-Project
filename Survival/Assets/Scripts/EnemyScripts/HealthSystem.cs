using UnityEngine;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    //[SerializeField] private Image healthBar;
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] private int dropAmount = 1;
    [SerializeField] private GameObject bloodVFX;

    private Animator animator;

    private EnemyAI ai;

    public int Health => health;
    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        ai = GetComponent<EnemyAI>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (bloodVFX != null)
        {
            GameObject spawnedVFX = Instantiate(bloodVFX, transform.position, Quaternion.identity);
        }

        if (animator != null)
        {
            animator.SetTrigger("GetHitTrigger");
        }

        if (health <= 0)
        {
            ai.ChangeState(new DeathState());
        }
    }

    public void Death()
    {
        if (dropAmount > 0 && itemToDrop != null)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                GameObject spawnedItem = Instantiate(itemToDrop);

                Vector3 offset = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0, 0.2f),
                    Random.Range(-0.5f, 0.5f)
                    );
                spawnedItem.transform.position = transform.position + offset;
            }
        }
        Destroy(gameObject);
    }
    public void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, maxHealth);
    }
}
