using System;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Factories;

namespace CodeBase.Infrastructure.Services
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