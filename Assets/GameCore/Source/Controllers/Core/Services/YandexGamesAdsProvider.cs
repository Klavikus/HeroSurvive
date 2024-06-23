using System;
using GameCore.Source.Controllers.Api.Providers;
using YG;

namespace GameCore.Source.Controllers.Core.Services
{
    public class YandexGamesAdsProvider : IAdsProvider
    {
        private const int ReRollAdId = 0;
        private const int RespawnAdId = 1;

        public event Action ReRollAdCompleted;
        public event Action RespawnAdCompleted;

        public YandexGamesAdsProvider()
        {
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
        }

        public void ShowReRollAd()
        {
            if (YandexGame.nowAdsShow)
                return;

            YandexGame.RewVideoShow(ReRollAdId);
        }

        public void ShowRespawnAd()
        {
            if (YandexGame.nowAdsShow)
                return;

            YandexGame.RewVideoShow(RespawnAdId);
        }

        private void OnRewardVideoEvent(int id)
        {
            switch (id)
            {
                case ReRollAdId:
                    ReRollAdCompleted?.Invoke();

                    break;
                case RespawnAdId:
                    RespawnAdCompleted?.Invoke();

                    break;
            }
        }
    }
}