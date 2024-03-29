using CodeBase.Domain.EntityComponents;
using CodeBase.Domain.StateMachines;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
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