public abstract class BaseState
{
    public abstract void SetContext(EnemyAI _enemy);
    public abstract void Enter();

    public abstract void Update();
    public abstract void Exit();
}
