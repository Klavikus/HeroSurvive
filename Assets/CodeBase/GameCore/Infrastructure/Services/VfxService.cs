using UnityEngine;
using UnityEngine.Pool;

namespace CodeBase.GameCore.Infrastructure.Services
{
    public class VfxService : IVfxService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private PoolableParticleSystem _killVfxPrefab;

        private IObjectPool<PoolableParticleSystem> _killVfxPool;

        public VfxService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _killVfxPrefab = _configurationProvider.VfxConfig.KillPrefab;
            _killVfxPool =
                new ObjectPool<PoolableParticleSystem>(CreateKillVfx, OnKillVfxGet, ActionOnRelease, ActionOnDestroy);
        }

        private void ActionOnDestroy(PoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed -= OnCompletedKillVfx;
            // killVfxInstance.Completed -= OnCompletedKillVfx;
            // _killVfxPool.Release(killVfxInstance);
        }

        public void HandleKill(Vector3 transformPosition)
        {
            var vfx = _killVfxPool.Get();
            if (vfx == null)
            {
                return;
            }

            vfx.transform.position = transformPosition;
            vfx.gameObject.SetActive(true);
        }

        public void Clear()
        {
            _killVfxPool.Clear();
        }

        private void ActionOnRelease(PoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.gameObject.SetActive(false);
        }

        private void OnKillVfxGet(PoolableParticleSystem killVfxInstance)
        {
        }

        private void OnCompletedKillVfx(PoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed -= OnCompletedKillVfx;
            _killVfxPool.Release(killVfxInstance);
        }

        private PoolableParticleSystem CreateKillVfx()
        {
            var poolObject = GameObject.Instantiate(_killVfxPrefab);
            poolObject.Completed += OnCompletedKillVfx;
            return poolObject;
        }
    }
}