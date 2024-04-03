using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations
{
    public class SimpleActionCounter : ActionCounter
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private float _countDuration;
        [SerializeField] private float _countStep;

        private float _currentValue;
        private float _targetValue;
        private bool _inProgress;
        private TimeSpan _stepDelay;
        private float _step;

        public override void Initialize(float initialValue)
        {
            _currentValue = initialValue;
            _countText.text = initialValue.ToString(CultureInfo.InvariantCulture);
        }

        public override async void Count(float targetValue)
        {
            _targetValue = targetValue;
            float delta = _targetValue - _currentValue;
            _step = delta / _countStep;
            _stepDelay = TimeSpan.FromSeconds(_countDuration / _countStep);

            if (_inProgress)
                return;

            _inProgress = true;

            InvokeCountStarted();

            while (Math.Abs(_targetValue - _currentValue) > _step)
            {
                _currentValue += _step;
                _countText.text = _currentValue.ToString(CultureInfo.InvariantCulture);
                await UniTask.Delay(_stepDelay);
            }

            _currentValue = _targetValue;
            _countText.text = _currentValue.ToString(CultureInfo.InvariantCulture);
            _inProgress = false;
            InvokeCountCompleted();
        }
    }
}