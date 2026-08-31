using UnityEngine;

public class AttackColliderCheck : MonoBehaviour
{
    [SerializeField] private float attackMagnitude = 0f;
    private int damage;
    [SerializeField] private float attackSphereRadius = 3f;
    [SerializeField] private GameObject particles;
    private Transform attackSphereTransform;

    private void Start()
    {
        if (gameObject.TryGetComponent(out EnemyAI ai))
        {
            damage = ai.Damage;
            attackSphereTransform = ai.AttackSphereProjector;
        }
    }
    public void CheckAttackCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(attackSphereTransform.position, attackSphereRadius);

        EventBus<CameraShakeEvent>.Raise(new CameraShakeEvent { magnitude = attackMagnitude });

        if (particles != null)
        {
            GameObject particle = Instantiate(particles, attackSphereTransform.position, Quaternion.identity);
        }
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == gameObject.layer) continue;

            if (collider.TryGetComponent(out IDamageable component))
            {
                component.TakeDamage(damage);
            }
        }
    }
}
