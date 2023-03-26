using System;

namespace CodeBase.Domain
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