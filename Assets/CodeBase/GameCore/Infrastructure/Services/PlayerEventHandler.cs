using System;
using CodeBase.GameCore.Domain.EntityComponents;

namespace CodeBase.GameCore.Infrastructure.Services
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