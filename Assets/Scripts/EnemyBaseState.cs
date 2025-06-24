public abstract class EnemyBaseState
{
    protected WolfAI wolf;

    public EnemyBaseState(WolfAI wolf)
    {
        this.wolf = wolf;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void FixedUpdate() { }
}
