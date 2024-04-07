using System;

namespace GameCore.Source.Controllers.Api.Providers
{
    public interface IAdsProvider
    {
        void ShowAds(Action onRewardCallback);
    }
}