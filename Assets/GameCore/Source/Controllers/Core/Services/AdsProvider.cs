using System;
using GameCore.Source.Controllers.Api.Providers;

namespace GameCore.Source.Controllers.Core.Services
{
    public class AdsProvider : IAdsProvider
    {
        public void ShowAds(Action onRewardCallback)
        {
            onRewardCallback?.Invoke();
        }
    }
}