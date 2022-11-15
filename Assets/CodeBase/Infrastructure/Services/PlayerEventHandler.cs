using System;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class PlayerEventHandler
    {
        public event Action Died;

        public void Initialize(IDamageable damageable)
        {
            damageable.Died += OnDied;
        }

        private void OnDied()
        {
            Debug.Log("PlayerEventHandler DIED");
            Died?.Invoke();
        }
    }
}