using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens
{
    public sealed class ChainedCompositionTween : TweenAction
    {
        [SerializeField] private TweenAction[] _tweenActionComponents;
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

            foreach (TweenAction actionComponent in _tweenActionComponents)
                actionComponent.Initialize();
        }

        public override async UniTask PlayForward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _inProgress = true;

            foreach (TweenAction actionComponent in _tweenActionComponents)
                await actionComponent.PlayForward();

            _inProgress = false;
        }

        public override async UniTask PlayBackward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _inProgress = true;

            foreach (TweenAction actionComponent in _tweenActionComponents)
                await actionComponent.PlayBackward();

            _inProgress = false;
        }

        public override void Cancel()
        {
            foreach (TweenAction actionComponent in _tweenActionComponents)
                actionComponent.Cancel();

            CancelTweens();

            _inProgress = false;
        }

        public override void SetForwardState()
        {
            foreach (TweenAction actionComponent in _tweenActionComponents)
                actionComponent.SetForwardState();
        }

        public override void SetBackwardState()
        {
            foreach (TweenAction actionComponent in _tweenActionComponents)
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