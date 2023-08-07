using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace CodeBase.Infrastructure
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private EventInstance _ambientInstance;
        private EventInstance _mainMenuAmbientInstance;

        public AudioPlayerService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public void PlayHit(Vector3 position) =>
            RuntimeManager.PlayOneShot(_configurationProvider.FMOD_HitReference, position);

        public void PlayUpgradeBuy() =>
            RuntimeManager.PlayOneShot(_configurationProvider.FMOD_UpgradeBuyReference);

        public void PlayPlayerDied() =>
            RuntimeManager.PlayOneShot(_configurationProvider.FMOD_PlayerDiedReference);

        public void PlayStartLevel() =>
            RuntimeManager.PlayOneShot(_configurationProvider.FMOD_StartLevelReference);

        public void PlayAmbient()
        {
            _ambientInstance.release();
            _ambientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_GameLoopAmbientReference);
            _ambientInstance.start();
        }

        public void StopAmbient()
        {
            _ambientInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _ambientInstance.release();
        }

        public void StartMainMenuAmbient()
        {
            _mainMenuAmbientInstance.release();
            _mainMenuAmbientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_MainMenuAmbientReference);
            _mainMenuAmbientInstance.start();
        }

        public void StopMainMenuAmbient()
        {
            _mainMenuAmbientInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _mainMenuAmbientInstance.release();
        }
    }
}