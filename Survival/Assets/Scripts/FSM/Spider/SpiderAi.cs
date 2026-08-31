using UnityEngine;

public class SpiderAi : EnemyAI
{

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Target = Player;

        if (CurrentState == null)
            ChangeState(new FlankingChaseState());
    }
    private void Update()
    {
        if (Target == null)
        {
            Target = Player;
        }
        CurrentState.Update();
    }
}
