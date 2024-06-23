using System;
using GameCore.Source.Controllers.Api.Providers;
using Modules.Common.Utils;
using YG;

namespace GameCore.Source.Controllers.Core.Services
{
    public class YandexGamesAdsProvider : IAdsProvider
    {
        private const int ReRollAdId = 0;
        private const int RespawnAdId = 1;
        private const int DoubleRewardAdId = 2;

        private readonly IMultiCallHandler _adCallHandler;

        public event Action ReRollAdCompleted;
        public event Action RespawnAdCompleted;
        public event Action DoubleRewardAdCompleted;
        public event Action AdStarted;
        public event Action AdClosed;

        public YandexGamesAdsProvider()
        {
            _adCallHandler = new MultiCallHandler();

            _adCallHandler.Called += () => AdStarted?.Invoke();
            _adCallHandler.Released += () => AdClosed?.Invoke();

            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.OpenVideoEvent += () => _adCallHandler.Call("RewardVideoAd");
            YandexGame.CloseVideoEvent += () => _adCallHandler.Release("RewardVideoAd");
            YandexGame.ErrorVideoEvent += () => _adCallHandler.Release("RewardVideoAd");

            YandexGame.OpenFullAdEvent += () => _adCallHandler.Call("InterAd");
            YandexGame.CloseFullAdEvent += () => _adCallHandler.Release("InterAd");
            YandexGame.ErrorFullAdEvent += () => _adCallHandler.Release("InterAd");
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

        public void ShowDoubleRewardAd()
        {
            if (YandexGame.nowAdsShow)
                return;

            YandexGame.RewVideoShow(DoubleRewardAdId);
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
                case DoubleRewardAdId:
                    DoubleRewardAdCompleted?.Invoke();

                    break;
            }
        }
    }
}