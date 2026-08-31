using UnityEngine;
using UnityEngine.AI;
public class SpiderAttackState : BaseState
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
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();
        }
        animator.SetTrigger("AttackTrigger");
    }
    public override void Update()
    {
        if (enemy.Player != null)
        {
            animator.SetTrigger("AttackTrigger");
            Vector3 lookDirection = enemy.Player.position - enemy.transform.position;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 12f);
            }
        }
        float distance = enemy.GetDistanceToTarget();

        if (distance > enemy.AttackDistance)
        {
            enemy.ChangeState(new FlankingChaseState());
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("AttackTrigger");
    }
}
