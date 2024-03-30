using System;

namespace GameCore.Source.Controllers.Api
{
    public interface IEntityState : IDisposable
    {
        void Enter();
        void Exit();
        void Update();
    }
}