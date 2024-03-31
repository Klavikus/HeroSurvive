using System;
using GameCore.Source.Domain.EntityComponents;

namespace GameCore.Source.Controllers.Core.Services
{
    public class PlayerEventHandler
    {
        public event Action Died;

        public void Initialize(IDamageable damageable) =>
            damageable.Died += OnDied;

        private void OnDied() =>
            Died?.Invoke();
    }
}