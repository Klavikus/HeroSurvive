using FMODUnity;
using UnityEngine;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IAudioPlayerService : IEnable, IDisable
    {
        void BindListenerTo(GameObject gameObject);
        void PlayHit(Vector3 position);
        void PlayUpgradeBuy();
        void PlayPlayerDied();
        void PlayStartLevel();
        void PlayAmbient();
        void StopAmbient();
        void StartMainMenuAmbient();
        void StopMainMenuAmbient();
        void PlayOneShot(EventReference reference);
        void PlayOneShot(EventReference reference, Vector3 position);
    }
}