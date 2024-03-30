using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.Common.Components.Implementations.Tweens
{
    public sealed class ChainedCompositionTween : TweenActionBaseComponent
    {
        [SerializeField] private TweenActionBaseComponent[] _tweenActionComponents;
        [SerializeField] private bool _lockDoubleInteraction;
        [SerializeField] private bool _initializeByComposition;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _inProgress;

        private void Start()
        {
            if (_initializeByComposition)
                return;

            Initialize();
        }

        public override void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            foreach (TweenActionBaseComponent actionComponent in _tweenActionComponents)
                actionComponent.Initialize();
        }

        public override async UniTask PlayForward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _inProgress = true;

            foreach (TweenActionBaseComponent actionComponent in _tweenActionComponents)
                await actionComponent.PlayForward();

            _inProgress = false;
        }

        public override async UniTask PlayBackward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _inProgress = true;

            foreach (TweenActionBaseComponent actionComponent in _tweenActionComponents)
                await actionComponent.PlayBackward();

            _inProgress = false;
        }

        public override void Cancel()
        {
            foreach (TweenActionBaseComponent actionComponent in _tweenActionComponents)
                actionComponent.Cancel();

            CancelTweens();

            _inProgress = false;
        }

        public override void SetForwardState()
        {
            foreach (TweenActionBaseComponent actionComponent in _tweenActionComponents)
                actionComponent.SetForwardState();
        }

        public override void SetBackwardState()
        {
            foreach (TweenActionBaseComponent actionComponent in _tweenActionComponents)
                actionComponent.SetBackwardState();
        }

        private void CancelTweens()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async void TestPlay()
        {
            await PlayForward();
            await PlayBackward();
        }
    }
}