using System;

namespace GameCore.Source.Controllers.Api.Providers
{
    public class AdsProvider : IAdsProvider
    {
        public void ShowAds(Action onRewardCallback)
        {
            onRewardCallback?.Invoke();
        }
    }
}