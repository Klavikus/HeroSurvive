using CodeBase.Domain.Enemies;
using CodeBase.Domain.EntityComponents;
using CodeBase.Domain.Enums;
using CodeBase.Domain.StateMachines;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Domain.EnemyStateMachine.States
{
    internal class HitEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;
        private readonly Damageable _damageable;
        private readonly IVfxService _vfxService;
        private readonly IAudioPlayerService _afxService;

        public HitEntityState(
            AnimationSynchronizer animationSynchronizer,
            EnemyAI enemyAI,
            Damageable damageable,
            IVfxService vfxService,
            IAudioPlayerService audioPlayerService)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
            _damageable = damageable;
            _vfxService = vfxService;
            _afxService = audioPlayerService;
        }

        public override void Enter()
        {
            base.Enter();
            _enemyAI.Stagger(_damageable.GetLastStagger());
            _animationSynchronizer.ChangeState(EntityAnimatorState.Hitted);
            _afxService.PlayHit(_damageable.transform.position);
        }
    }
}