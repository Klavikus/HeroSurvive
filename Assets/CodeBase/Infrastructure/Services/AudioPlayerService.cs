using System;
using System.Collections;
using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace CodeBase.Infrastructure
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private readonly IGamePauseService _gamePauseService;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICoroutineRunner _coroutineRunner;
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
        }

        public void Initialize(IPersistentDataService persistentDataService)
        {
            _ambientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_GameLoopAmbientReference);
            _mainMenuAmbientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_MainMenuAmbientReference);

            _gamePauseService.PauseStarted += OnPauseStarted;
            _gamePauseService.PauseEnded += OnPauseEnded;
        }

        private void OnPauseStarted()
        {
            RuntimeManager.PauseAllEvents(true);
            RuntimeManager.CoreSystem.mixerSuspend();
        }

        private void OnPauseEnded()
        {
            RuntimeManager.PauseAllEvents(false);
            RuntimeManager.CoreSystem.mixerResume();
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
            _coroutineRunner.StartCoroutine(DelayedStartGameLoop());
        }

        public void StopAmbient()
        {
            _coroutineRunner.StartCoroutine(DelayedStopGameLoop());
        }

        public void StartMainMenuAmbient()
        {
            _coroutineRunner.StartCoroutine(DelayedStartMainMenu());
        }

        public void StopMainMenuAmbient()
        {
            _coroutineRunner.StartCoroutine(DelayedStopMainMenu());
        }

        public void PlayThunder()
        {
            RuntimeManager.PlayOneShot(_configurationProvider.FMOD_Thunder);
        }

        public void PlayOneShot(EventReference reference) => RuntimeManager.PlayOneShot(reference);
        public void PlayOneShot(EventReference reference, Vector3 position) => RuntimeManager.PlayOneShot(reference, position);

        private IEnumerator DelayedStartMainMenu()
        {
            yield return new WaitForSeconds(0.1f);
            _mainMenuAmbientInstance.start();
        }

        private IEnumerator DelayedStopMainMenu()
        {
            yield return new WaitForSeconds(0.1f);
            _mainMenuAmbientInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }

        private IEnumerator DelayedStartGameLoop()
        {
            yield return new WaitForSeconds(0.1f);
            _ambientInstance.start();
        }

        private IEnumerator DelayedStopGameLoop()
        {
            yield return new WaitForSeconds(0.1f);
            _ambientInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}