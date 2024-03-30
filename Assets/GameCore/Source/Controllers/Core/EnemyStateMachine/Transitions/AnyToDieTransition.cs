using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.StateMachines;
using GameCore.Source.Domain.EntityComponents;

namespace GameCore.Source.Controllers.Core.EnemyStateMachine.Transitions
{
    public sealed class AnyToDieTransition : Transition
    {
        private readonly Damageable _damageable;

        public AnyToDieTransition(
            IEntityState nextEntityState,
            Damageable damageable)
            : base(nextEntityState)
        {
            _damageable = damageable;
            _damageable.Died += MoveNextState;
        }

        public override void OnDispose()
        {
            base.OnDispose();
            _damageable.Died -= MoveNextState;
        }
    }
}