using System;

namespace CodeBase.Infrastructure.Services
{
    public interface IGamePauseService : IService
    {
        event Action PauseStarted;
        event Action PauseEnded;
        bool IsPaused { get; }
        void StartPauseByRewarded();
        void StopPauseByRewarded();
        void StartPauseByMenu();
        void StopPauseByRaceMenu();
        void StartPauseByInterstitial();
        void StopPauseByInterstitial();
    }
}