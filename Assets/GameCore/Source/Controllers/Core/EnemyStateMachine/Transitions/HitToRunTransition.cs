using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.StateMachines;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions
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