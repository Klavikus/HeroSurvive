using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
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
            Died?.Invoke();
        }
    }
}