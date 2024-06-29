using System.Collections;
using System.Diagnostics;
using FMODUnity;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.Services;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameCore.Source.Controllers.Core.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private readonly IGamePauseService _gamePauseService;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly AudioConfig _audioConfig;
        private EventInstance _ambientInstance;
        private EventInstance _mainMenuAmbientInstance;

        public AudioPlayerService
        (
            IConfigurationProvider configurationProvider,
            ICoroutineRunner coroutineRunner,
            IGamePauseService gamePauseService
        )
        {
            _gamePauseService = gamePauseService;
            _configurationProvider = configurationProvider;
            _coroutineRunner = coroutineRunner;

            _audioConfig = configurationProvider.AudioConfig;
        }

        public void BindListenerTo(GameObject gameObject)
        {
            Object.FindObjectOfType<StudioListener>().BindAttenuation(gameObject);
        }

        public void Initialize()
        {
            // _ambientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_GameLoopAmbientReference);
            // _mainMenuAmbientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_MainMenuAmbientReference);

            // _gamePauseService.PauseStarted += OnPauseStarted;
            // _gamePauseService.PauseEnded += OnPauseEnded;
        }

        private void OnPauseStarted()
        {
            // RuntimeManager.PauseAllEvents(true);
            // RuntimeManager.CoreSystem.mixerSuspend();
        }

        private void OnPauseEnded()
        {
            // RuntimeManager.PauseAllEvents(false);
            // RuntimeManager.CoreSystem.mixerResume();
        }

        public void PlayHit(Vector3 position)
        {
            RuntimeManager.PlayOneShot(_audioConfig.HitReference, position);
        }

        public void PlayUpgradeBuy()
        {
            RuntimeManager.PlayOneShot(_audioConfig.UpgradeBuyReference);
        }

        public void PlayPlayerDied()
        {
            RuntimeManager.PlayOneShot(_audioConfig.PlayerDiedReference);
        }

        public void PlayStartLevel()
        {
            RuntimeManager.PlayOneShot(_audioConfig.StartLevelReference);
        }

        public void PlayAmbient()
        {
        }

        public void StopAmbient()
        {
        }

        public void StartMainMenuAmbient()
        {
        }

        public void StopMainMenuAmbient()
        {
        }

        public void PlayOneShot(EventReference reference)
        {
            RuntimeManager.PlayOneShot(reference);
        }

        public void PlayOneShot(EventReference reference, Vector3 position)
        {
            RuntimeManager.PlayOneShot(reference, position);
        }
    }
}