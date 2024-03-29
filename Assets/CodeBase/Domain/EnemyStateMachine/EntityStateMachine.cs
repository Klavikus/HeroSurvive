using System;
using System.Collections.Generic;
using CodeBase.Domain.EnemyStateMachine.States;
using CodeBase.Domain.EnemyStateMachine.Transitions;

namespace CodeBase.Domain.EnemyStateMachine
{
    public class EntityStateMachine : IDisposable
    {
        private readonly IEntityState _startEntityState;
        private readonly List<Transition> _transitions;
        private IEntityState _currentEntityState;

        public EntityStateMachine(IEntityState startEntityState, List<Transition> transitions)
        {
            _startEntityState = startEntityState;
            _transitions = transitions;
        }

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

        public void Dispose()
        {
            foreach (Transition transition in _transitions)
            {
                transition.Dispose();
            }
        }
    }
}