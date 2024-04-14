using System;
using Modules.UIComponents.Runtime.Implementations.Tweens;
using Modules.UIComponents.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UIComponents.Runtime.Implementations.Buttons
{
    public class ActionButton : MonoBehaviour, IActionButton
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _upAndDown;

        [SerializeField] private TweenAction _actionComponent;
        [SerializeField] private TweenAction _focusActionComponent;

        public event Action Clicked;

        private bool _isInteractionLocked;
        private bool _isFocused;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Initialize()
        {
            _actionComponent.Initialize();
            _focusActionComponent?.Initialize();
        }

        public void SetInteractionLock(bool isLock)
        {
            _button.interactable = isLock == false;
            _isInteractionLocked = isLock;
        }

        public void Focus()
        {
            _isFocused = true;
            _focusActionComponent?.PlayForward();
        }

        public void Unfocus()
        {
            _focusActionComponent?.Cancel();
            _isFocused = false;
        }

        public async void OnButtonClicked()
        {
            if (_isInteractionLocked)
                return;

            _isInteractionLocked = true;

            await _actionComponent.PlayForward();

            if (_upAndDown == false)
            {
                _isInteractionLocked = false;

                Clicked?.Invoke();
                _actionComponent.SetBackwardState();

                return;
            }

            await _actionComponent.PlayBackward();

            _isInteractionLocked = false;

            Clicked?.Invoke();
        }
    }
}