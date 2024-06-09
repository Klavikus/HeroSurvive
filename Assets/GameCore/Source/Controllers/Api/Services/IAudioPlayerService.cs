using FMODUnity;
using UnityEngine;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface IAudioPlayerService
    {
        void PlayHit(Vector3 position);
        void PlayUpgradeBuy();
        void PlayPlayerDied();
        void PlayStartLevel();
        void PlayAmbient();
        void StopAmbient();
        void StartMainMenuAmbient();
        void StopMainMenuAmbient();
        void PlayThunder();
        void PlayOneShot(EventReference reference);
        void PlayOneShot(EventReference reference, Vector3 position);
    }
}