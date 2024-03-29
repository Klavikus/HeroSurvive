using System;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Services
{
    public class PoolableParticleSystem : MonoBehaviour
    {
        public event Action<PoolableParticleSystem> Completed;

        private void OnDisable() => Completed?.Invoke(this);
    }
}