using UnityEngine;

namespace CodeBase.Domain
{
    class DieEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;
        private readonly Enemy _enemy;

        public DieEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI, Enemy enemy)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _animationSynchronizer.ChangeState(EntityAnimatorState.Died);
            _enemyAI.enabled = false;

            //TODO: change Destroy for BackToPool
            _enemy.InvokeBackToPool();
            // GameObject.Destroy(_enemyAI.gameObject, 0.5f);
        }
    }
}