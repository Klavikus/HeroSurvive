using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Modules.UIComponents.Runtime.Implementations.Tweens.ConfigsSo;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens
{
    public sealed class RectTransformMoveTween : TweenActionBaseComponent
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TwoSidedVector2TweenData _tweenData;
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
                ? _tweenData.Forward.Value.magnitude / _tweenData.Forward.Duration
                : _tweenData.Forward.Duration;

            await _rectTransform
                .DOAnchorPos(_initialPosition + _tweenData.Forward.Value, duration)
                .SetEase(_tweenData.Forward.Ease)
                .SetUpdate(_tweenData.IgnoreTimeScale)
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

            _rectTransform.anchoredPosition = _tweenData.Forward.Value;

            _inProgress = true;

            float duration = _useDurationAsSpeed
                ? _tweenData.Backward.Value.magnitude / _tweenData.Backward.Duration
                : _tweenData.Backward.Duration;
            
            await _rectTransform
                .DOAnchorPos(_initialPosition + _tweenData.Backward.Value, duration)
                .SetEase(_tweenData.Backward.Ease)
                .SetUpdate(_tweenData.IgnoreTimeScale)
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
            _rectTransform.anchoredPosition = _initialPosition + _tweenData.Forward.Value;
        }

        public override void SetBackwardState()
        {
            CancelTweens();
            _rectTransform.anchoredPosition = _initialPosition + _tweenData.Backward.Value;
        }

        private void CancelTweens()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}