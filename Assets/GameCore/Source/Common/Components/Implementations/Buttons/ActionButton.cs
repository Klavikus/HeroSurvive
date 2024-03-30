using System;
using Source.Common.Components.Implementations.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Common.Components.Implementations.Buttons
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _upAndDown;

        [SerializeField] private TweenActionBaseComponent _actionComponent;

        public event Action Clicked;

        private bool _isInteractionLocked;

        private void OnEnable() =>
            _button.onClick.AddListener(OnButtonClicked);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        public void Initialize() =>
            _actionComponent.Initialize();

        public void SetInteractionLock(bool isLock) =>
            _isInteractionLocked = isLock;

        private async void OnButtonClicked()
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