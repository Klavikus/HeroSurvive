using System;
using CodeBase.Domain.EntityComponents;

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
            Died?.Invoke();
        }
    }
}