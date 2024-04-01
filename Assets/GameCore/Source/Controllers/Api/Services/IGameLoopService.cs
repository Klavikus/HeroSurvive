using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IGameLoopService
    {
        event Action CloseLevelInvoked;
        event Action PlayerResurrectInvoked;
        event Action PlayerDied;
        void InvokeLevelClose();
        void NotifyPlayerDeath();
    }
}