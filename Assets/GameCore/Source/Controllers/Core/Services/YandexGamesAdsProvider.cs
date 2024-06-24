using System;
using GameCore.Source.Controllers.Api.Providers;
using Modules.Common.Utils;
using YG;

namespace GameCore.Source.Controllers.Core.Services
{
    public class YandexGamesAdsProvider : IAdsProvider
    {
        private const string s_rewardVideoCallKey = "RewardVideoAd";
        private const string s_interCallKey = "InterAd";

        private const int s_reRollAdId = 0;
        private const int s_respawnAdId = 1;
        private const int s_doubleRewardAdId = 2;

        private readonly IMultiCallHandler _adCallHandler;

        public YandexGamesAdsProvider()
        {
            _adCallHandler = new MultiCallHandler();

            _adCallHandler.Called += () => AdStarted?.Invoke();
            _adCallHandler.Released += () => AdClosed?.Invoke();

            //TODO: Fix this
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.OpenVideoEvent += () => _adCallHandler.Call(s_rewardVideoCallKey);
            YandexGame.CloseVideoEvent += () => _adCallHandler.Release(s_rewardVideoCallKey);
            YandexGame.ErrorVideoEvent += () => _adCallHandler.Release(s_rewardVideoCallKey);

            YandexGame.OpenFullAdEvent += () => _adCallHandler.Call(s_interCallKey);
            YandexGame.CloseFullAdEvent += () => _adCallHandler.Release(s_interCallKey);
            YandexGame.ErrorFullAdEvent += () => _adCallHandler.Release(s_interCallKey);
        }

        public event Action ReRollAdCompleted;
        public event Action RespawnAdCompleted;
        public event Action DoubleRewardAdCompleted;
        public event Action AdStarted;
        public event Action AdClosed;

        public bool IsAdInProgress => _adCallHandler.IsCalled;

        public void ShowReRollAd()
        {
            if (YandexGame.nowAdsShow)
                return;

            _adCallHandler.Call(s_rewardVideoCallKey);
            YandexGame.RewVideoShow(s_reRollAdId);
        }

        public void ShowRespawnAd()
        {
            if (YandexGame.nowAdsShow)
                return;

            _adCallHandler.Call(s_rewardVideoCallKey);
            YandexGame.RewVideoShow(s_respawnAdId);
        }

        public void ShowDoubleRewardAd()
        {
            if (YandexGame.nowAdsShow)
                return;

            _adCallHandler.Call(s_rewardVideoCallKey);
            YandexGame.RewVideoShow(s_doubleRewardAdId);
        }

        private void OnRewardVideoEvent(int id)
        {
            switch (id)
            {
                case s_reRollAdId:
                    ReRollAdCompleted?.Invoke();

                    break;
                case s_respawnAdId:
                    RespawnAdCompleted?.Invoke();

                    break;
                case s_doubleRewardAdId:
                    DoubleRewardAdCompleted?.Invoke();

                    break;
            }
        }
    }
}