using System.Collections;
using CodeBase.Domain.Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable;
        [SerializeField] private float _transitionMaxDelta;
        [SerializeField] private Image _fillImage;

        private float _viewHealth;
        private Coroutine _updateViewCoroutine;

        private void OnEnable() => _damageable.HealthChanged += OnDamageableChanged;

        private void OnDisable() => _damageable.HealthChanged -= OnDamageableChanged;

        private void Start()
        {
            _fillImage.fillAmount = 1f;
        }

        private void OnDamageableChanged(int currentHealth, float healthPercent)
        {
            if (_updateViewCoroutine != null)
                StopCoroutine(_updateViewCoroutine);

            _updateViewCoroutine = StartCoroutine(UpdateView(healthPercent));
        }

        private IEnumerator UpdateView(float healthPercentage)
        {
            while (_viewHealth != healthPercentage)
            {
                _viewHealth = Mathf.MoveTowards(_viewHealth, healthPercentage, _transitionMaxDelta * Time.deltaTime);
                _fillImage.fillAmount = _viewHealth;
                yield return null;
            }
        }
    }
}