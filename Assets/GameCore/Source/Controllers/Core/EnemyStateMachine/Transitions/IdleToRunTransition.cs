using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.StateMachines;
using GameCore.Source.Domain.Enemies;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions
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