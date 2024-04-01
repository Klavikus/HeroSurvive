using System;
using GameCore.Source.Controllers.Api.Services;

namespace GameCore.Source.Controllers.Core.Services
{
    public class GameLoopService : IGameLoopService
    {
        public event Action CloseLevelInvoked;
        public event Action PlayerResurrectInvoked;
        public event Action PlayerDied;

        public void InvokeLevelClose() =>
            CloseLevelInvoked?.Invoke();

        public void ResurrectPlayer() =>
            PlayerResurrectInvoked?.Invoke();

        public void NotifyPlayerDeath() =>
            PlayerDied?.Invoke();
    }
}