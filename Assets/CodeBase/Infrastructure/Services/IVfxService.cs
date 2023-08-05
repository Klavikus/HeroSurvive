using System;
using UnityEngine;
using UnityEngine.Pool;

namespace CodeBase.Infrastructure.Services
{
    public interface IVfxService : IService
    {
        void HandleKill(Vector3 transformPosition);
    }

    public class VfxService : IVfxService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private PoolableParticleSystem _killVfxPrefab;

        private IObjectPool<PoolableParticleSystem> _killVfxPool;

        public VfxService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _killVfxPrefab = _configurationProvider.VfxConfig.KillPrefab;
            _killVfxPool = new ObjectPool<PoolableParticleSystem>(CreateKillVfx, OnKillVfxGet, ActionOnRelease);
        }

        public void HandleKill(Vector3 transformPosition)
        {
            var vfx = _killVfxPool.Get();
            vfx.transform.position = transformPosition;
            vfx.gameObject.SetActive(true);
        }

        private void ActionOnRelease(PoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed -= OnCompletedKillVfx;
            _killVfxPool.Release(killVfxInstance);
        }

        private void OnKillVfxGet(PoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed += OnCompletedKillVfx;
        }

        private void OnCompletedKillVfx(PoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed -= OnCompletedKillVfx;
            killVfxInstance.gameObject.SetActive(false);
            _killVfxPool.Release(killVfxInstance);
        }

        private PoolableParticleSystem CreateKillVfx() => GameObject.Instantiate(_killVfxPrefab);
    }

    [Serializable]
    public class VfxConfig
    {
        [field: SerializeField] public PoolableParticleSystem KillPrefab { get; private set; }
    }
}