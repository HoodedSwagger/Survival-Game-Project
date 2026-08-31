using UnityEngine;
using UnityEngine.AI;

public class AttackState : BaseState
{
    private EnemyAI enemy;
    private NavMeshAgent agent;
    private Animator animator;

    private float attackTimer = 0f;
    public override void SetContext(EnemyAI _enemy)
    {
        enemy = _enemy;
        agent = enemy.GetComponent<NavMeshAgent>();
        animator = _enemy.GetComponent<Animator>();
    }
    public override void Enter()
    {
        if (animator != null)
        {
            animator.Play("Attack");
            animator.SetTrigger("AttackTrigger");
        }
    }
    public override void Update()
    {
        float distance = enemy.GetDistanceToTarget();
        if (distance >= enemy.IdleDistance)
        {
            enemy.ChangeState(new IdleState());
        }
        else if (distance > enemy.AttackDistance)
        {
            enemy.ChangeState(new ChaseState());
        }
        else
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= 1)
            {
                Attack();
                attackTimer = 0;
            }
        }
    }
    public override void Exit()
    {
    }

    private void Attack()
    {
        //Collider[] colliders = Physics.OverlapSphere(enemy.AttackSphereProjector.position, 3f);
        //foreach (Collider collider in colliders)
        //{
        //    if (collider.gameObject.layer == enemy.gameObject.layer) continue;
        //    if (collider.TryGetComponent(out IDamageable component))
        //    {
        //        component.TakeDamage(enemy.Damage);
        //    }
        //}
    }
}
