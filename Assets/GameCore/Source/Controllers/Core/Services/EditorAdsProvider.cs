using System;
using GameCore.Source.Controllers.Api.Providers;

namespace GameCore.Source.Controllers.Core.Services
{
    public class EditorAdsProvider : IAdsProvider
    {
        public event Action ReRollAdCompleted;
        public event Action RespawnAdCompleted;

        public void ShowReRollAd()
        {
            ReRollAdCompleted?.Invoke();
        }

        public void ShowRespawnAd()
        {
            RespawnAdCompleted?.Invoke();
        }
    }
}