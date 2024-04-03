using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api;
using UnityEngine;
using UnityEngine.Pool;

namespace GameCore.Source.Controllers.Core.Services
{
    public class VfxService : IVfxService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly GameObject _killVfxPrefab;
        private readonly IObjectPool<IPoolableParticleSystem> _killVfxPool;

        public VfxService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _killVfxPrefab = _configurationProvider.VfxConfig.KillPrefab;
            _killVfxPool = new ObjectPool<IPoolableParticleSystem>(CreateKillVfx, OnKillVfxGet, ActionOnRelease, ActionOnDestroy);
        }

        private void ActionOnDestroy(IPoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed -= OnCompletedKillVfx;
        }

        public void HandleKill(Vector3 transformPosition)
        {
            IPoolableParticleSystem vfx = _killVfxPool.Get();

            if (vfx == null)
                return;

            vfx.GameObject.transform.position = transformPosition;
            vfx.GameObject.SetActive(true);
        }

        public void Clear()
        {
            _killVfxPool.Clear();
        }

        private void ActionOnRelease(IPoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.GameObject.SetActive(false);
        }

        private void OnKillVfxGet(IPoolableParticleSystem killVfxInstance)
        {
        }

        private void OnCompletedKillVfx(IPoolableParticleSystem killVfxInstance)
        {
            killVfxInstance.Completed -= OnCompletedKillVfx;
            _killVfxPool.Release(killVfxInstance);
        }

        private IPoolableParticleSystem CreateKillVfx()
        {
            GameObject poolObject = Object.Instantiate(_killVfxPrefab);
            IPoolableParticleSystem poolableParticleSystem = poolObject.GetComponent<IPoolableParticleSystem>();
            poolableParticleSystem.Completed += OnCompletedKillVfx;

            return poolableParticleSystem;
        }
    }
}