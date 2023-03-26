namespace CodeBase.Domain
{
    class HitToIdleTransition : Transition
    {
        private readonly EnemyAI _enemyAI;

        public HitToIdleTransition(IEntityState nextEntityState, EnemyAI enemyAI) : base(nextEntityState)
        {
            _enemyAI = enemyAI;
            _enemyAI.StaggerOut += OnStaggerOut;
        }

        private void OnStaggerOut()
        {
            if (_enemyAI.IsMoving == false)
                MoveNextState();
        }

        public override void Update()
        {
        }
    }
}