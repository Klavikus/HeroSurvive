using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Views
{
    [Serializable]
    public sealed class View
    {
        private enum ViewType
        {
            SingleSprite,
            SpriteSheet,
        }

        [SerializeField] private ViewType _viewType;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _shouldHideOnEnd;
        [SerializeField] private float _oneCyclePlayTime;

        private SpriteRenderer _renderer;
        private int _currentSpriteId;
        private WaitForSeconds _animationDelay;
        private bool _canPlay;

        public void Initialize(SpriteRenderer renderer)
        {
            _renderer = renderer;
            _animationDelay = new WaitForSeconds(_oneCyclePlayTime / _sprites.Length);
            _canPlay = true;
        }

        public IEnumerator Run()
        {
            switch (_viewType)
            {
                case ViewType.SingleSprite:
                    _renderer.sprite = _sprites[0];

                    break;
                case ViewType.SpriteSheet:
                    yield return PlaySpriteSheetAnimation();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator PlaySpriteSheetAnimation()
        {
            while (_canPlay)
            {
                _renderer.sprite = _sprites[_currentSpriteId];

                if (++_currentSpriteId == _sprites.Length)
                    if (_isLooped)
                        _currentSpriteId = 0;
                    else
                        _canPlay = false;

                yield return _animationDelay;
            }

            if (_shouldHideOnEnd)
                _renderer.enabled = false;
        }
    }
}