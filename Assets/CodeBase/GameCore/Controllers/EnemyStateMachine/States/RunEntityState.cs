using CodeBase.GameCore.Controllers.StateMachines;
using CodeBase.GameCore.Domain.Enemies;
using CodeBase.GameCore.Domain.EntityComponents;
using CodeBase.GameCore.Domain.Enums;

namespace CodeBase.GameCore.Controllers.EnemyStateMachine.States
{
    class RunEntityState : EntityState
    {
        private readonly AnimationSynchronizer _animationSynchronizer;
        private readonly EnemyAI _enemyAI;

        public RunEntityState(AnimationSynchronizer animationSynchronizer, EnemyAI enemyAI)
        {
            _animationSynchronizer = animationSynchronizer;
            _enemyAI = enemyAI;
        }

        public override void Enter()
        {
            base.Enter();

            _enemyAI.enabled = true;
            _animationSynchronizer.ChangeState(EntityAnimatorState.Walk);
        }
    }
}