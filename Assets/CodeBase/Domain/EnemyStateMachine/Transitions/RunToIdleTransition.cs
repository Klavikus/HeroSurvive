namespace CodeBase.Domain
{
    class RunToIdleTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public RunToIdleTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StopMoving += MoveNextState;
        }

        public override void Update()
        {

        }
    }
}