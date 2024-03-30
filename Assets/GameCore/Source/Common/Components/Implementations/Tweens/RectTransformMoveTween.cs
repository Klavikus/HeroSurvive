using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Source.Common.Components.Implementations.Tweens
{
    public sealed class RectTransformMoveTween : TweenActionBaseComponent
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2TweenData _forwardPlayData;
        [SerializeField] private Vector2TweenData _backwardPlayData;
        [SerializeField] private bool _lockDoubleInteraction;
        [SerializeField] private bool _initializeByComposition;
        [SerializeField] private bool _activateBackwardAfterForward;
        [SerializeField] private bool _useDurationAsSpeed;

        private CancellationTokenSource _cancellationTokenSource;

        private Vector2 _initialPosition;
        private bool _inProgress;

        private void Start()
        {
            if (_initializeByComposition)
                return;

            Initialize();
        }

        private void OnDestroy()
        {
            Cancel();
        }

        public override void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _initialPosition = _rectTransform.anchoredPosition;
        }

        public override async UniTask PlayForward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _rectTransform.anchoredPosition = _initialPosition;

            _inProgress = true;

            float duration = _useDurationAsSpeed
                ? _forwardPlayData.Value.magnitude / _forwardPlayData.Duration
                : _forwardPlayData.Duration;

            await _rectTransform
                .DOAnchorPos(_initialPosition + _forwardPlayData.Value, duration)
                .SetEase(_forwardPlayData.Ease)
                .WithCancellation(_cancellationTokenSource.Token);

            _inProgress = false;

            if (_activateBackwardAfterForward)
                await PlayBackward();
        }

        public override async UniTask PlayBackward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _rectTransform.anchoredPosition = _forwardPlayData.Value;

            _inProgress = true;

            float duration = _useDurationAsSpeed
                ? _backwardPlayData.Value.magnitude / _backwardPlayData.Duration
                : _backwardPlayData.Duration;
            
            await _rectTransform
                .DOAnchorPos(_initialPosition + _backwardPlayData.Value, duration)
                .SetEase(_backwardPlayData.Ease)
                .WithCancellation(_cancellationTokenSource.Token);

            _inProgress = false;
        }

        public override void Cancel()
        {
            CancelTweens();
            _rectTransform.DOKill();
            _inProgress = false;
        }

        public override void SetForwardState()
        {
            CancelTweens();
            _rectTransform.anchoredPosition = _initialPosition + _forwardPlayData.Value;
        }

        public override void SetBackwardState()
        {
            CancelTweens();
            _rectTransform.anchoredPosition = _initialPosition + _backwardPlayData.Value;
        }

        private void CancelTweens()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}