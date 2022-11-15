using System;
using CodeBase.Domain.Data;
using CodeBase.ForSort;
using EntityState = CodeBase.Domain.EnemyStateMachine.States.EntityState;

namespace CodeBase.Domain.Enemies
{
    internal class HitEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;

        public HitEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
        }
        
        public override void Enter()
        {
            base.Enter();
            _enemyAI.Stagger();
            _animationSynchronizer.ChangeState(CodeBase.ForSort.EntityState.Hitted);
        }
    }
}