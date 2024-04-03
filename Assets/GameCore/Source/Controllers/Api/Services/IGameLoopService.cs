using System;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IGameLoopService
    {
        event Action CloseLevelInvoked;
        event Action PlayerResurrectInvoked;
        event Action PlayerDied;
        bool PlayerIsAlive { get; }
        void InvokeLevelClose();
        void NotifyPlayerDeath();
        void NotifyPlayerRespawn();
        void ResurrectPlayer();
        event Action PlayerResurrected;
    }
}