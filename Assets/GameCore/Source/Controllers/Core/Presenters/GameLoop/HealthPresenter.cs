using System;
using System.Collections;
using GameCore.Source.Domain.EntityComponents;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Presenters.GameLoop
{
    public class HealthPresenter : IPresenter
    {
        private readonly IHealthView _view;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IDamageable _damageable;

        private float _viewHealth;
        private Coroutine _updateViewCoroutine;

        public HealthPresenter(IHealthView view, ICoroutineRunner coroutineRunner)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _coroutineRunner = coroutineRunner;
            _damageable = _view.Target.GetComponent<IDamageable>() ??
                          throw new ArgumentNullException(nameof(IDamageable));
        }

        public void Enable()
        {
            _damageable.HealthChanged += OnHealthChanged;
            _view.FillImage.fillAmount = 1f;
            OnHealthChanged(0, _damageable.CurrentHealthPercent);
        }

        public void Disable()
        {
            _damageable.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int currentHealth, float healthPercent)
        {
            if (_updateViewCoroutine != null)
                _coroutineRunner.StopCoroutine(_updateViewCoroutine);

            _updateViewCoroutine = _coroutineRunner.StartCoroutine(UpdateView(healthPercent));
        }

        private IEnumerator UpdateView(float healthPercentage)
        {
            while (Math.Abs(_viewHealth - healthPercentage) > Single.Epsilon)
            {
                _viewHealth = Mathf.MoveTowards(_viewHealth, healthPercentage,
                    _view.TransitionMaxDelta * Time.deltaTime);
                _view.FillImage.fillAmount = _viewHealth;

                yield return null;
            }
        }
    }
}