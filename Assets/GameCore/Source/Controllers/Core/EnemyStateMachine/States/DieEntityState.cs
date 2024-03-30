using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.StateMachines;
using GameCore.Source.Domain.Enemies;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Domain.Enums;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.States
{
    class DieEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;
        private readonly Enemy _enemy;
        private readonly IVfxService _vfxService;

        public DieEntityState(
            AnimationSynchronizer animationSynchronizer,
            EnemyAI enemyAI,
            Enemy enemy,
            IVfxService vfxService)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
            _enemy = enemy;
            _vfxService = vfxService;
        }

        public override void Enter()
        {
            base.Enter();

            if (_enemy == null)
                return;

            _animationSynchronizer.ChangeState(EntityAnimatorState.Died);
            _vfxService.HandleKill(_enemy.transform.position);
            _enemyAI.enabled = false;

            //TODO: change Destroy for BackToPool
            _enemy?.InvokeBackToPool();
            // GameObject.Destroy(_enemyAI.gameObject, 0.5f);
        }
    }
}