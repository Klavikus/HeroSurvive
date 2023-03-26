using System;

namespace CodeBase.Domain
{
    public interface IEntityState
    {
        event Action<IEntityState> NeedChangeState;
        void Enter();
        void Exit();
        void Update();
    }
}