using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.StateMachines;
using GameCore.Source.Domain.Enemies;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions
{
    class HitToIdleTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public HitToIdleTransition(
            IEntityState nextEntityState,
            EnemyAI enemyAI)
            : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StaggerOut += OnStaggerOut;
        }

        private void OnStaggerOut()
        {
            if (_enemyAI.IsMoving == false)
                MoveNextState();
        }

        public override void OnDispose()
        {
            base.OnDispose();
            _enemyAI.StaggerOut -= OnStaggerOut;
        }
    }
}