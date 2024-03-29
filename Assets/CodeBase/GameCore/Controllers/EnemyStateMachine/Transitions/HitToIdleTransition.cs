using CodeBase.GameCore.Controllers.StateMachines;
using CodeBase.GameCore.Domain.Enemies;

namespace CodeBase.GameCore.Controllers.EnemyStateMachine.Transitions
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