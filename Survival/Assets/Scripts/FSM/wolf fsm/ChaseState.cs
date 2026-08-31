using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    private EnemyAI enemy;
    private NavMeshAgent agent;
    private Animator animator;

    private NavMeshPath path;
    public override void SetContext(EnemyAI _enemy)
    {
        enemy = _enemy;
        agent = enemy.GetComponent<NavMeshAgent>();
        animator = _enemy.GetComponent<Animator>();
    }
    public override void Enter()
    {
        if (animator != null) 
            animator.Play("Run");
    }
    public override void Update()
    {
        
        agent.SetDestination(enemy.Target.position);
        float distance = enemy.GetDistanceToTarget();

        if (distance <= enemy.AttackDistance)
        {
            enemy.ChangeState(new AttackState());
        }

        if (agent.path.status == NavMeshPathStatus.PathPartial)
        {
            agent.CalculatePath(enemy.Target.position, agent.path);

            Vector3 lastCorner = agent.path.corners[agent.path.corners.Length - 1];

            Collider[] colliders = Physics.OverlapSphere(lastCorner, 5f, 1 << 12);

            if (colliders.Length > 0)
            {
                enemy.Target = colliders[0].transform;
            }
        }
    }
    public override void Exit()
    {
    }
}
