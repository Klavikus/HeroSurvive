using System;
using CodeBase.Domain.Data;

namespace CodeBase.Infrastructure.Services
{
    public interface IGameLoopService : IService
    {
        event Action<HeroData> LevelInvoked;
        event Action LevelCloseInvoked;
        void InvokeLevelStart(HeroData heroData);
        void InvokeLevelClose();
        void Start();
        void Stop();
    }
}