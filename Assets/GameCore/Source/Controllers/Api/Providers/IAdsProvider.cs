using System;

namespace GameCore.Source.Controllers.Api.Providers
{
    public interface IAdsProvider
    {
        event Action ReRollAdCompleted;
        event Action RespawnAdCompleted;
        event Action DoubleRewardAdCompleted;
        event Action AdStarted;
        event Action AdClosed;
        void ShowReRollAd();
        void ShowRespawnAd();
        void ShowDoubleRewardAd();
    }
}