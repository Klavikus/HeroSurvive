using CodeBase.Domain.Data;
using CodeBase.Domain.EnemyStateMachine.States;

class HitToRunTransition : Transition
{
    private readonly EnemyAI _enemyAI;

    public HitToRunTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
    {
        _enemyAI = enemyAI;
        _enemyAI.StaggerOut += OnStaggerOut;
    }

    private void OnStaggerOut()
    {
        if (_enemyAI.IsMoving)
            MoveNextState();
    }

    public override void Update()
    {
    }
}