using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Modules.UIComponents.Runtime.Implementations.Tweens.ConfigsSo;
using TMPro;
using UnityEngine;

namespace Modules.UIComponents.Runtime.Implementations
{
    public class SimpleActionCounter : ActionCounter
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private ActionCounterConfig _config;

        private float _countDuration;
        private float _countStep;
        private float _currentValue;
        private float _targetValue;
        private bool _inProgress;
        private TimeSpan _stepDelay;
        private float _stepValue;
        private string _format;

        public override void Initialize(float initialValue)
        {
            _currentValue = initialValue;
            _countText.text = initialValue.ToString(CultureInfo.InvariantCulture);

            _countDuration = _config.CountDuration;
            _countStep = _config.CountStep;
            _format = _config.Format;
        }

        public override async void Count(float targetValue)
        {
            _targetValue = targetValue;
            float delta = Mathf.Abs(_targetValue - _currentValue);
            _stepValue = delta / _countStep;
            _stepDelay = TimeSpan.FromSeconds(_countDuration / _countStep);

            if (_inProgress)
                return;

            _inProgress = true;

            InvokeCountStarted();

            while (Math.Abs(_targetValue - _currentValue) > _stepValue)
            {
                if (_targetValue < _currentValue)
                    _currentValue -= _stepValue;
                else
                    _currentValue += _stepValue;

                _countText.text = _currentValue.ToString(_format, CultureInfo.InvariantCulture);

                await UniTask.Delay(_stepDelay, _config.DelayType);
            }

            _currentValue = _targetValue;
            _countText.text = _currentValue.ToString(CultureInfo.InvariantCulture);
            _inProgress = false;
            InvokeCountCompleted();
        }
    }
}