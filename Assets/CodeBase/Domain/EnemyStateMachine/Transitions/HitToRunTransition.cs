using CodeBase.Domain.Enemies;
using CodeBase.Domain.StateMachines;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    class HitToRunTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public HitToRunTransition(
            IEntityState nextEntityState,
            EnemyAI enemyAI)
            : base(nextEntityState)
        {
            _enemyAI = enemyAI;
        }

        public override void OnUpdate()
        {
            if (_enemyAI.IsStaggered)
                return;

            MoveNextState();
        }
    }
}