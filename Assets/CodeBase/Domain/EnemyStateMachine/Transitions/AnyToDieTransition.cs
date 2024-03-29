using CodeBase.Domain.EnemyStateMachine.States;
using CodeBase.Domain.EntityComponents;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    class AnyToDieTransition : Transition
    {
        private readonly Damageable _damageable;

        public AnyToDieTransition(IEntityState nextEntityState, Damageable damageable) : base(nextEntityState)
        {
            _damageable = damageable;
            _damageable.Died += MoveNextState;
        }

        public override void Update()
        {
        }
    }
}