using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
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