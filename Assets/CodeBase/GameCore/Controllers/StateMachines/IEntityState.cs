using System;

namespace CodeBase.GameCore.Controllers.StateMachines
{
    public interface IEntityState : IDisposable
    {
        void Enter();
        void Exit();
        void Update();
    }
}