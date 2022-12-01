using CodeBase.Domain.Data;
using CodeBase.Domain.EnemyStateMachine.States;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    class HitToRunTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public HitToRunTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
        {
            _enemyAI = enemyAI;
        }

        public override void Update()
        {
            if (_enemyAI.IsStaggered == false) 
                MoveNextState();
        }
    }
}