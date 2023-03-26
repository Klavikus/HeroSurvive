namespace CodeBase.Domain
{
    internal class HitEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;
        private readonly Damageable _damageable;

        public HitEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI, Damageable damageable)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
            _damageable = damageable;
        }

        public override void Enter()
        {
            base.Enter();
            _enemyAI.Stagger(_damageable.GetLastStagger());
            _animationSynchronizer.ChangeState(EntityAnimatorState.Hitted);
        }
    }
}