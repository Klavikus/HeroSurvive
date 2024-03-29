using CodeBase.Domain.Enemies;
using CodeBase.Domain.StateMachines;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
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