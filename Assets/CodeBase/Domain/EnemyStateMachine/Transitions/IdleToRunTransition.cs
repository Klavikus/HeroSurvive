using CodeBase.Domain.Enemies;
using CodeBase.Domain.StateMachines;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    class IdleToRunTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public IdleToRunTransition(
            IEntityState nextEntityState,
            EnemyAI enemyAI)
            : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StartMoving += MoveNextState;
        }

        public override void OnDispose() =>
            _enemyAI.StartMoving -= MoveNextState;
    }
}