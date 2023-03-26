namespace CodeBase.Domain
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