using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.StateMachines;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions
{
    class RunToIdleTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public RunToIdleTransition(
            IEntityState nextEntityState,
            EnemyAI enemyAI)
            : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StopMoving += MoveNextState;
        }

        public override void OnDispose() =>
            _enemyAI.StopMoving -= MoveNextState;
    }
}