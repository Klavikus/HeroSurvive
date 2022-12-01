using CodeBase.Domain.Data;
using CodeBase.Domain.EntityComponents;
using UnityEngine;

namespace CodeBase.Domain.EnemyStateMachine.States
{
    class DieEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;

        public DieEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
        }

        public override void Enter()
        {
            base.Enter();
            _animationSynchronizer.ChangeState(ForSort.EntityState.Died);
            _enemyAI.enabled = false;

            //TODO: change Destroy for BackToPool
            GameObject.Destroy(_enemyAI.gameObject, 0.5f);
        }
    }
}