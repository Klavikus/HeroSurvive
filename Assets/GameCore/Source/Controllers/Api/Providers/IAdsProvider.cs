using System;

namespace GameCore.Source.Controllers.Api.Providers
{
    public interface IAdsProvider
    {
        event Action ReRollAdCompleted;
        event Action RespawnAdCompleted;
        void ShowReRollAd();
        void ShowRespawnAd();
    }
}