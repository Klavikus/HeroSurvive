using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class PoolableParticleSystem : MonoBehaviour
    {
        public event Action<PoolableParticleSystem> Completed;

        private void OnDisable() => Completed?.Invoke(this);
    }
}