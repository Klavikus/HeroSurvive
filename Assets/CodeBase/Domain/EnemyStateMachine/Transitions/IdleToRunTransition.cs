using CodeBase.Domain.Data;
using CodeBase.Domain.EnemyStateMachine.States;

class IdleToRunTransition : Transition
{
    private readonly EnemyAI _enemyAI;

    public IdleToRunTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
    {
        _enemyAI = enemyAI;
        _enemyAI.StartMoving += MoveNextState;
    }

    public override void Update()
    {
    }
}