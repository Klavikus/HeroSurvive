using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Domain
{
    internal class HitEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;
        private readonly Damageable _damageable;
        private readonly IVfxService _vfxService;
        private readonly IAudioPlayerService _sfxService;

        public HitEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI, Damageable damageable)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
            _damageable = damageable;
            _vfxService = AllServices.Container.AsSingle<IVfxService>();
            _sfxService = AllServices.Container.AsSingle<IAudioPlayerService>();
        }

        public override void Enter()
        {
            base.Enter();
            _enemyAI.Stagger(_damageable.GetLastStagger());
            _animationSynchronizer.ChangeState(EntityAnimatorState.Hitted);
            _sfxService.PlayHit(_damageable.transform.position);
        }
    }
}