using UnityEngine;
using UnityEngine.AI;

public class FlankingChaseState : BaseState
{
    private EnemyAI enemy;
    private NavMeshAgent agent;
    private Animator animator;

    private float pathRecalculateTimer = 0.25f;
    private float timer = 0f;

    private int flankSign = 1;

    public override void SetContext(EnemyAI _enemy)
    {
        enemy = _enemy;
        agent = enemy.GetComponent<NavMeshAgent>();
        animator = _enemy.GetComponent<Animator>();
    }

    public override void Enter()
    {
        flankSign = Random.value > 0.5f ? 1 : -1;

        UpdateFlankDestination();
    }

    public override void Update()
    {
        Vector3 lookDirection = enemy.Player.position - enemy.transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 10);
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.position);
        if (distanceToPlayer <= enemy.AttackDistance)
        {
            enemy.ChangeState(new SpiderAttackState());
            return;
        }

        timer += Time.deltaTime;
        if (timer >= pathRecalculateTimer)
        {
            timer = 0f;
            if (distanceToPlayer < 5f)
            {
                agent.SetDestination(enemy.Player.position);
            }
            else
            {
                UpdateFlankDestination();
            }
        }
    }

    private void UpdateFlankDestination()
    {
        if (enemy.Player == null) return;

        Vector3 flankDirection = enemy.Player.right * flankSign;

        Vector3 targetFlankPoint = enemy.Player.position + (flankDirection * 4f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetFlankPoint, out hit, 1.5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            flankSign = -flankSign;
        }
    }

    public override void Exit()
    {
        if (agent.isActiveAndEnabled)
            agent.ResetPath();
    }
}
