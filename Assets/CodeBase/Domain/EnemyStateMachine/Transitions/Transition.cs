using System;
using CodeBase.Domain.EnemyStateMachine.States;

namespace CodeBase.Domain.EnemyStateMachine.Transitions
{
    public abstract class Transition : IDisposable
    {
        private readonly IEntityState _nextEntityState;

        protected Transition(IEntityState nextEntityState)
        {
            _nextEntityState = nextEntityState;
        }

        public event Action<IEntityState> NeedChangeState;

        public abstract void Update();

        protected void MoveNextState() => NeedChangeState?.Invoke(_nextEntityState);

        public void Dispose() => _nextEntityState?.Dispose();
    }
}