﻿using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations.Tweens
{
    public sealed class RectTransformSizeTween : TweenActionBaseComponent
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2TweenData _forwardPlayData;
        [SerializeField] private Vector2TweenData _backwardPlayData;
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
            
            await _rectTransform
                .DOSizeDelta(_initialSize * _forwardPlayData.Value, _forwardPlayData.Duration)
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

            _rectTransform.sizeDelta = _initialSize * _forwardPlayData.Value;

            _inProgress = true;

            await _rectTransform
                .DOSizeDelta(_initialSize * _backwardPlayData.Value, _backwardPlayData.Duration)
                .SetEase(_backwardPlayData.Ease)
                .WithCancellation(_cancellationTokenSource.Token);

            _inProgress = false;
        }

        public override void SetForwardState()
        {
            CancelTweens();
            _rectTransform.sizeDelta = _initialSize + _forwardPlayData.Value;
        }

        public override void SetBackwardState()
        {
            CancelTweens();
            _rectTransform.sizeDelta = _initialSize + _backwardPlayData.Value;
        }

        private void CancelTweens()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}