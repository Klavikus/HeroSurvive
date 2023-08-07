using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface IAudioPlayerService : IService
    {
        void PlayHit(Vector3 position);
        void PlayUpgradeBuy();
        void PlayPlayerDied();
        void PlayStartLevel();
        void PlayAmbient();
        void StopAmbient();
        void StartMainMenuAmbient();
        void StopMainMenuAmbient();
    }
}