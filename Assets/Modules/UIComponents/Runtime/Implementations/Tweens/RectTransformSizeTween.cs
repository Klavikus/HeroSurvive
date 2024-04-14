using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Modules.UIComponents.Runtime.Implementations.Tweens.ConfigsSo;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens
{
    public sealed class RectTransformSizeTween : TweenAction
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TwoSidedVector2TweenData _tweenData;
        [SerializeField] private bool _lockDoubleInteraction = true;
        [SerializeField] private bool _initializeByComposition = true;
        [SerializeField] private bool _activateBackwardAfterForward;
        [SerializeField] private bool _autoRestart;

        private CancellationTokenSource _cancellationTokenSource;

        private Vector2 _initialSize;
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
            _initialSize = _rectTransform.sizeDelta;
        }

        public override void Cancel()
        {
            CancelTweens();
            _rectTransform.DOKill();
            _inProgress = false;
        }

        public override async UniTask PlayForward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _rectTransform.sizeDelta = _initialSize;

            _inProgress = true;

            if (_autoRestart)
            {
                await _rectTransform
                    .DOSizeDelta(_initialSize * _tweenData.Forward.Value, _tweenData.Forward.Duration)
                    .SetEase(_tweenData.Forward.Ease)
                    .SetUpdate(_tweenData.IgnoreTimeScale)
                    .SetLoops(-1, LoopType.Yoyo)
                    .WithCancellation(_cancellationTokenSource.Token);
            }
            else
            {
                await _rectTransform
                    .DOSizeDelta(_initialSize * _tweenData.Forward.Value, _tweenData.Forward.Duration)
                    .SetEase(_tweenData.Forward.Ease)
                    .SetUpdate(_tweenData.IgnoreTimeScale)
                    .WithCancellation(_cancellationTokenSource.Token);
            }

            _inProgress = false;

            if (_activateBackwardAfterForward)
                await PlayBackward();
        }

        public override async UniTask PlayBackward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _rectTransform.sizeDelta = _initialSize * _tweenData.Forward.Value;

            _inProgress = true;

            await _rectTransform
                .DOSizeDelta(_initialSize * _tweenData.Backward.Value, _tweenData.Backward.Duration)
                .SetEase(_tweenData.Backward.Ease)
                .SetUpdate(_tweenData.IgnoreTimeScale)
                .WithCancellation(_cancellationTokenSource.Token);

            _inProgress = false;
        }

        public override void SetForwardState()
        {
            CancelTweens();
            _rectTransform.sizeDelta = _initialSize + _tweenData.Forward.Value;
        }

        public override void SetBackwardState()
        {
            CancelTweens();
            _rectTransform.sizeDelta = _initialSize + _tweenData.Backward.Value;
        }

        private void CancelTweens()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}