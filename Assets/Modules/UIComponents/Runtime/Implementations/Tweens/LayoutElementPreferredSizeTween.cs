using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Modules.UIComponents.Runtime.Implementations.Tweens.ConfigsSo;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UIComponents.Runtime.Implementations.Tweens
{
    public sealed class LayoutElementPreferredSizeTween : TweenActionBaseComponent
    {
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private TwoSidedVector2TweenData _tweenData;
        [SerializeField] private bool _lockDoubleInteraction;
        [SerializeField] private bool _initializeByComposition;
        [SerializeField] private bool _activateBackwardAfterForward;

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
            _initialSize = new Vector2(_layoutElement.preferredWidth, _layoutElement.preferredHeight);
        }

        public override void Cancel()
        {
            CancelTweens();
            _layoutElement.DOKill();
            _inProgress = false;
        }

        public override async UniTask PlayForward()
        {
            if (_lockDoubleInteraction && _inProgress)
                return;

            CancelTweens();

            _layoutElement.preferredWidth = _initialSize.x;
            _layoutElement.preferredHeight = _initialSize.y;

            _inProgress = true;

            await _layoutElement
                .DOPreferredSize(_initialSize * _tweenData.Forward.Value, _tweenData.Forward.Duration)
                .SetEase(_tweenData.Forward.Ease)
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

            _layoutElement.preferredWidth = _initialSize.x * _tweenData.Forward.Value.x;
            _layoutElement.preferredHeight = _initialSize.y * _tweenData.Forward.Value.y;

            _inProgress = true;

            await _layoutElement
                .DOPreferredSize(_initialSize * _tweenData.Backward.Value, _tweenData.Backward.Duration)
                .SetEase(_tweenData.Backward.Ease)
                .WithCancellation(_cancellationTokenSource.Token);

            _inProgress = false;
        }

        public override void SetForwardState()
        {
            CancelTweens();
            _layoutElement.preferredWidth = _initialSize.x * _tweenData.Forward.Value.x;
            _layoutElement.preferredHeight = _initialSize.y * _tweenData.Forward.Value.y;
        }

        public override void SetBackwardState()
        {
            CancelTweens();
            _layoutElement.preferredWidth = _initialSize.x * _tweenData.Backward.Value.x;
            _layoutElement.preferredHeight = _initialSize.y * _tweenData.Backward.Value.y;
        }

        private void CancelTweens()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}