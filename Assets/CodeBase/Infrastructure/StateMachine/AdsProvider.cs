using System;
using Agava.YandexGames;
using CodeBase.ForSort;

namespace CodeBase.Infrastructure.StateMachine
{
    public class AdsProvider : IAdsProvider
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public event Action Opened;
        public event Action Rewarded;
        public event Action Closed;
        public event Action Initialized;
        public bool IsInitialized => YandexGamesSdk.IsInitialized;

        public AdsProvider(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Initialize()
        {
            YandexGamesSdk.CallbackLogging = true;

            _coroutineRunner.StartCoroutine(YandexGamesSdk.Initialize(onSuccessCallback: OnSuccessCallback));
        }

        private void OnSuccessCallback()
        {
            Initialized?.Invoke();
        }

        public void ShowAds(Action OnOpenCallback, Action OnCloseCallback, Action OnRewardCallback)
        {
            if (IsInitialized == false)
                return;

            VideoAd.Show(
                onOpenCallback: OnOpenCallback,
                onCloseCallback: OnCloseCallback,
                onRewardedCallback: OnRewardCallback);
        }

        private void OnOpenCallback() => Opened?.Invoke();
        private void OnRewardedCallback() => Rewarded?.Invoke();
        private void OnCloseCallback() => Closed?.Invoke();
    }
}