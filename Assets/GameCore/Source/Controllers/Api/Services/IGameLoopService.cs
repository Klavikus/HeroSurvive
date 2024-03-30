using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IGameLoopService
    {
        event Action<HeroData> LevelInvoked;
        event Action LevelCloseInvoked;
        void InvokeLevelStart(HeroData heroData);
        void InvokeLevelClose();
        void Start();
        void Stop();
    }
}