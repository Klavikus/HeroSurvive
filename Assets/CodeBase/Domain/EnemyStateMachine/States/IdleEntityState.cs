using CodeBase.Domain.Data;
using CodeBase.Domain.EntityComponents;

namespace CodeBase.Domain.EnemyStateMachine.States
{
    internal class IdleEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;

        public IdleEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
        }

        public override void Enter()
        {
            base.Enter();
            
            _enemyAI.enabled = true;
            // _animationSynchronizer.ChangeState(ForSort.EntityState.Idle);
        }
    }
}