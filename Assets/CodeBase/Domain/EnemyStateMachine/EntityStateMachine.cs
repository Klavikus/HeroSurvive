namespace CodeBase.Domain
{
    public class EntityStateMachine
    {
        private readonly IEntityState _startEntityState;
        private IEntityState _currentEntityState;

        public EntityStateMachine(IEntityState startEntityState) => 
            _startEntityState = startEntityState;

        public void Reset() => ChangeEntityState(_startEntityState);

        public void Update() => _currentEntityState?.Update();

        private void ChangeEntityState(IEntityState nextEntityState)
        {
            if (_currentEntityState == nextEntityState)
                return;

            if (_currentEntityState != null)
            {
                _currentEntityState.Exit();
                _currentEntityState.NeedChangeState -= ChangeEntityState;
            }

            _currentEntityState = nextEntityState;
            _currentEntityState.Enter();
            _currentEntityState.NeedChangeState += ChangeEntityState;
        }
    }
}