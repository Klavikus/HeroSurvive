using CodeBase.Domain.EnemyStateMachine.States;
using CodeBase.Domain.EntityComponents;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    class AnyToHitTransition : Transition
    {
        private readonly Damageable _damageable;

        public AnyToHitTransition(IEntityState nextEntityState, Damageable damageable) : base(nextEntityState)
        {
            _damageable = damageable;
            _damageable.DamageTaken += OnHitTaken;
        }

        private void OnHitTaken(int damage, float stagger) => MoveNextState();

        public override void Update()
        {
        }
    }
}