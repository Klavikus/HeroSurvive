namespace CodeBase.Domain
{
    class IdleToRunTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public IdleToRunTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StartMoving += MoveNextState;
        }

        public override void Update()
        {
        }
    }
}