using CodeBase.Domain.Enemies;
using CodeBase.Domain.EnemyStateMachine.States;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    class RunToIdleTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public RunToIdleTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StopMoving += MoveNextState;
        }

        public override void Update()
        {

        }
    }
}