using UnityEngine;

public class IdleState : BaseState
{
    private EnemyAI enemy; 
    private Animator animator;
    public override void SetContext(EnemyAI _enemy)
    {
        enemy = _enemy;
        animator = _enemy.GetComponent<Animator>();
    }
    public override void Enter()
    {
        animator.Play("Idle");
    }
    public override void Update()
    {
        if (enemy.GetDistanceToTarget() <= enemy.AgroDistance)
        {
            enemy.ChangeState(new ChaseState());
        }
    }
    public override void Exit()
    {
        
    }
}
