using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Presentation.ViewComponents
{
    public class SelectedTweener : MonoBehaviour
    {
        [SerializeField] private GameObject _tweenTriggerContainer;
        [SerializeField] private GameObject _tweenTarget;
        [SerializeField] private TweenStrategy _tweenStrategy;

        private ITweenTrigger _tweenTrigger;
        private bool _isInitialized;
        private Tween _currentTween;
        private Vector3 _initialScale;

        private void Awake()
        {
            _tweenTrigger = _tweenTriggerContainer.GetComponent<ITweenTrigger>();
            _tweenTrigger.Showed += OnTriggerShowed;
            _tweenTrigger.Hided += OnTriggerHided;
            _initialScale = _tweenTarget.transform.localScale;
            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _tweenTrigger.Showed -= OnTriggerShowed;
            _tweenTrigger.Hided -= OnTriggerHided;
            KillCurrentTween();
        }

        private void OnTriggerShowed(ITweenTrigger tweenTrigger)
        {
            KillCurrentTween();
            _currentTween = _tweenTarget.transform
                .DOScale(_initialScale * _tweenStrategy.ScaleUpModifier, _tweenStrategy.LoopDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
            _currentTween.onKill += OnShowTweenKilled;
        }

        private void OnShowTweenKilled()
        {
            _currentTween.onKill -= OnShowTweenKilled;
            _tweenTarget.transform.localScale = _initialScale;
        }

        private void OnTriggerHided(ITweenTrigger tweenTrigger)
        {
            KillCurrentTween();
            _tweenTarget.transform.localScale = _initialScale;
        }

        private void KillCurrentTween()
        {
            if (_currentTween != null)
                _currentTween.Kill();
        }

        private void OnValidate()
        {
            if (_tweenTriggerContainer.TryGetComponent(out ITweenTrigger tweenTrigger) == false)
                throw new Exception($"The {_tweenTriggerContainer} must implement {nameof(ITweenTrigger)}.");
        }
    }
}