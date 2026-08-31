using UnityEngine;

public class DeathState : BaseState
{
    private EnemyAI enemy;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;

    private float attackTimer = 0f;
    public override void SetContext(EnemyAI _enemy)
    {
        enemy = _enemy;
        agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = _enemy.GetComponent<Animator>();
    }
    public override void Enter()
    {
        if (animator != null)
        {
            animator.Play("Death");
        }
    }
    public override void Update()
    {
         
    }
    public override void Exit()
    {
    }

}
