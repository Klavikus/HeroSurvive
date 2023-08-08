using System;

namespace CodeBase.Domain
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