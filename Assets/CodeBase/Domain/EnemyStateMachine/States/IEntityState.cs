using System;

namespace CodeBase.Domain
{
    public interface IEntityState : IDisposable
    {
        event Action<IEntityState> NeedChangeState;
        void Enter();
        void Exit();
        void Update();
    }
}