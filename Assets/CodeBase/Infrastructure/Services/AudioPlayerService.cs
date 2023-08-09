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
        private IGamePauseService _gamePauseService;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICoroutineRunner _coroutineRunner;
        private EventInstance _ambientInstance;
        private EventInstance _mainMenuAmbientInstance;

        private bool _isMutedByUser;
        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _sfxBus;

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
            ISaveLoadService saveLoadService = AllServices.Container.AsSingle<ISaveLoadService>();
            _ambientInstance = RuntimeManager.CreateInstance(_configurationProvider.FMOD_GameLoopAmbientReference);
            _mainMenuAmbientInstance =
                RuntimeManager.CreateInstance(_configurationProvider.FMOD_MainMenuAmbientReference);

            _isMutedByUser = bool.Parse(saveLoadService.GetData(GameConstants.MuteStatus.ToString(), "False"));

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");

            _gamePauseService.PauseStarted += OnPauseStarted;
            _gamePauseService.PauseEnded += OnPauseEnded;
        }

        private void OnPauseStarted()
        {
            _masterBus.setPaused(true);
            _musicBus.setPaused(true);
            _sfxBus.setPaused(true);
        }

        private void OnPauseEnded()
        {
            // if (_isMutedByUser)
            //     return;
            _masterBus.setPaused(false);
            _musicBus.setPaused(false);
            _sfxBus.setPaused(false);
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
            // _ambientInstance.start();
            _coroutineRunner.StartCoroutine(DelayedStartGameLoop());
        }

        public void StopAmbient()
        {
            // _ambientInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _coroutineRunner.StartCoroutine(DelayedStopGameLoop());
        }

        public void StartMainMenuAmbient()
        {
            // _mainMenuAmbientInstance.start();
            _coroutineRunner.StartCoroutine(DelayedStartMainMenu());
        }

        public void StopMainMenuAmbient()
        {
            // _mainMenuAmbientInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _coroutineRunner.StartCoroutine(DelayedStopMainMenu());
        }

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