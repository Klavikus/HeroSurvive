using System;

namespace CodeBase.Infrastructure.Services
{
    public interface IGamePauseService : IService
    {
        event Action PauseStarted;
        event Action PauseEnded;
        void StartPauseByRewarded();
        void StopPauseByRewarded();
        void StartPauseByRaceMenu();
        void StopPauseByRaceMenu();
        void StartPauseByInterstitial();
        void StopPauseByInterstitial();
    }
}