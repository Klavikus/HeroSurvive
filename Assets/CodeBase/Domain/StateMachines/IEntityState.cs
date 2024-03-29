using System;

namespace CodeBase.Domain.StateMachines
{
    public interface IEntityState : IDisposable
    {
        void Enter();
        void Exit();
        void Update();
    }
}