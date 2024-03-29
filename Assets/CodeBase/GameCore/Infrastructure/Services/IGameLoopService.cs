using System;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Infrastructure.Factories;

namespace CodeBase.GameCore.Infrastructure.Services
{
    public interface IGameLoopService : IService, IInitializeable
    {
        event Action<HeroData> LevelInvoked;
        event Action LevelCloseInvoked;
        void InvokeLevelStart(HeroData heroData);
        void InvokeLevelClose();
        void Start();
        void Stop();
    }
}