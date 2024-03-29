using CodeBase.GameCore.Controllers.StateMachines;
using CodeBase.GameCore.Domain.Enemies;

namespace CodeBase.GameCore.Controllers.EnemyStateMachine.Transitions
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