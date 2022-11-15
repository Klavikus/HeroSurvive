using System;
using System.Collections;
using CodeBase.Domain.Data;
using UnityEngine;

namespace CodeBase.Domain.Enemies
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        private int _maxHealth;
        private int _currentHealth;
        private int _healthRegeneration;
        private WaitForSeconds _healthRegenerationDelay;
        private Coroutine _regenerationCoroutine;
        private DamageableData _damageableData;

        public event Action<int, float> HealthChanged;
        public event Action<int> DamageTaken;
        public event Action<int> HealTaken;
        public event Action Died;

        public bool CanReceiveDamage => _currentHealth > 0;
        private float CurrentHealthPercent => (float) _currentHealth / _maxHealth;

        public void Initialize(DamageableData damageableData)
        {
            _damageableData = damageableData;
            _maxHealth = _damageableData.MaxHealth;
            _currentHealth = _maxHealth;
            _healthRegeneration = (int) (_maxHealth * _damageableData.HealthRegenerationPercent);
            _healthRegenerationDelay = new WaitForSeconds(_damageableData.RegenerationDelayInSeconds);
            HealthChanged?.Invoke(_currentHealth, CurrentHealthPercent);

            if (_regenerationCoroutine != null)
                StopCoroutine(_regenerationCoroutine);

            if (_healthRegeneration > 0)
                _regenerationCoroutine = StartCoroutine(HealthRegeneration());
        }

        public void UpdateProgression(float stageProgressionModifier)
        {
            _damageableData.UpdateProgression(stageProgressionModifier);
            _maxHealth = _damageableData.MaxHealth;
            _currentHealth = _maxHealth;
            _healthRegeneration = (int) (_maxHealth * _damageableData.HealthRegenerationPercent);
            _healthRegenerationDelay = new WaitForSeconds(_damageableData.RegenerationDelayInSeconds);
            HealthChanged?.Invoke(_currentHealth, CurrentHealthPercent);
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException($"{nameof(damage)} should be greater then 0");

            _currentHealth -= damage;
            DamageTaken?.Invoke(damage);
            HealthChanged?.Invoke(_currentHealth, CurrentHealthPercent);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Died?.Invoke();
            }
        }

        public void RestoreHealth(int healAmount)
        {
            if (healAmount < 0)
                throw new ArgumentException($"{nameof(healAmount)} should be greater then 0");

            _currentHealth += healAmount;
            HealTaken?.Invoke(healAmount);
            HealthChanged?.Invoke(_currentHealth, CurrentHealthPercent);

            if (_currentHealth > _maxHealth) 
                _currentHealth = _maxHealth;
        }

        private IEnumerator HealthRegeneration()
        {
            while (true)
            {
                yield return _healthRegenerationDelay;
                RestoreHealth(_healthRegeneration);
            }
        }
    }
}