using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.StateMachines;
using GameCore.Source.Domain.EntityComponents;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions
{
    public sealed class AnyToHitTransition : Transition
    {
        private readonly Damageable _damageable;

        public AnyToHitTransition(
            IEntityState nextEntityState,
            Damageable damageable)
            : base(nextEntityState)
        {
            _damageable = damageable;
            _damageable.DamageTaken += OnHitTaken;
        }

        private void OnHitTaken(int damage, float stagger) =>
            MoveNextState();

        public override void OnDispose() =>
            _damageable.DamageTaken -= OnHitTaken;
    }
}