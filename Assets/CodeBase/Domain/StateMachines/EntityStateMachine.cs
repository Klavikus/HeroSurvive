using System;
using System.Collections.Generic;

namespace CodeBase.Domain.StateMachines
{
    public class EntityStateMachine : IDisposable
    {
        private readonly IEntityState _startEntityState;
        private readonly IEnumerable<EntityState> _states;

        private IEntityState _currentEntityState;

        public EntityStateMachine(IEntityState startEntityState, IEnumerable<EntityState> states)
        {
            _startEntityState = startEntityState;
            _states = states;
        }

        public void Reset() =>
            ChangeState(_startEntityState);

        public void Update() =>
            _currentEntityState?.Update();

        public void ChangeState(IEntityState nextEntityState)
        {
            if (_currentEntityState == nextEntityState)
                return;

            _currentEntityState?.Exit();
            _currentEntityState = nextEntityState;
            _currentEntityState.Enter();
        }

        public void Dispose()
        {
            foreach (EntityState entityState in _states) 
                entityState.Dispose();
        }
    }
}