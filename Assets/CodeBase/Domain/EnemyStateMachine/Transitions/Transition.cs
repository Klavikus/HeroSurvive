using System;
using CodeBase.Domain.EnemyStateMachine.States;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    abstract class Transition
    {
        private readonly IEntityState _nextEntityState;

        protected Transition(IEntityState nextEntityState)
        {
            _nextEntityState = nextEntityState;
        }

        public event Action<IEntityState> NeedChangeState;
    
        public abstract void Update();

        protected void MoveNextState() => NeedChangeState?.Invoke(_nextEntityState);
    }
}