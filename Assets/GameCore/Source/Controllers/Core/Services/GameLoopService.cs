using System;
using GameCore.Source.Controllers.Api.Services;

namespace GameCore.Source.Controllers.Core.Services
{
    public class GameLoopService : IGameLoopService
    {
        public event Action CloseLevelInvoked;
        public event Action PlayerResurrectInvoked;
        public event Action PlayerResurrected;
        public event Action PlayerDied;

        public bool PlayerIsAlive { get; private set; }

        public void InvokeLevelClose() =>
            CloseLevelInvoked?.Invoke();

        public void ResurrectPlayer() =>
            PlayerResurrectInvoked?.Invoke();

        public void NotifyPlayerDeath()
        {
            PlayerIsAlive = false;
            PlayerDied?.Invoke();
        }

        public void NotifyPlayerRespawn()
        {
            PlayerIsAlive = true;
            PlayerResurrected?.Invoke();
        }
    }
}