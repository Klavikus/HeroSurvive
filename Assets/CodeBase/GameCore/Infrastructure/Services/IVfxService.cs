using System;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Services
{
    public interface IVfxService : IService
    {
        void HandleKill(Vector3 transformPosition);
        void Clear();
    }

    [Serializable]
    public class VfxConfig
    {
        [field: SerializeField] public PoolableParticleSystem KillPrefab { get; private set; }
    }
}