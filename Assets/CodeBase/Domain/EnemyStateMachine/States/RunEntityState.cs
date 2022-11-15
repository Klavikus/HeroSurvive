using CodeBase.Domain.Data;
using CodeBase.ForSort;
using EntityState = CodeBase.Domain.EnemyStateMachine.States.EntityState;

class RunEntityState : EntityState
{
    private readonly AnimationSynchronizer _animationSynchronizer;
    private readonly EnemyAI _enemyAI;

    public RunEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI)
    {
        _animationSynchronizer = animationSynchronizer;
        _enemyAI = enemyAI;
    }

    public override void Enter()
    {
        base.Enter();

        _enemyAI.enabled = true;
        _animationSynchronizer.ChangeState(CodeBase.ForSort.EntityState.Walk);
    }
}