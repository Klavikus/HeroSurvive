using System;
using GameCore.Source.Presentation.Api;
using UnityEngine;

namespace GameCore.Source.Presentation.Core
{
    public class PoolableParticleSystem : MonoBehaviour, IPoolableParticleSystem
    {
        public event Action<IPoolableParticleSystem> Completed;
        public GameObject GameObject => gameObject;

        private void OnDisable() => Completed?.Invoke(this);
    }
}