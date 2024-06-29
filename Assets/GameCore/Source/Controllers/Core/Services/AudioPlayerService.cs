using FMOD.Studio;
using FMODUnity;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.Services;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;
using EventInstance = System.Diagnostics.EventInstance;

namespace GameCore.Source.Controllers.Core.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private readonly IGamePauseService _gamePauseService;
        private readonly IModelProvider _modelProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly AudioConfig _audioConfig;
        private EventInstance _ambientInstance;
        private EventInstance _mainMenuAmbientInstance;
        private SettingsModel _settingsModel;
        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _sfxBus;

        public AudioPlayerService
        (
            IConfigurationProvider configurationProvider,
            ICoroutineRunner coroutineRunner,
            IGamePauseService gamePauseService,
            IModelProvider modelProvider
        )
        {
            _gamePauseService = gamePauseService;
            _modelProvider = modelProvider;
            _configurationProvider = configurationProvider;
            _coroutineRunner = coroutineRunner;

            _audioConfig = configurationProvider.AudioConfig;
        }

        public void Enable()
        {
            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");

            _settingsModel = _modelProvider.Get<SettingsModel>();

            _gamePauseService.Paused += OnPauseStarted;
            _gamePauseService.Resumed += OnPauseEnded;

            _settingsModel.MuteChanged += OnMuteChanged;
            _settingsModel.MasterVolumeChanged += OnMasterVolumeChanged;
            _settingsModel.MusicVolumeChanged += OnMusicVolumeChanged;
            _settingsModel.SfxVolumeChanged += OnSfxVolumeChanged;
            
            ApplySettings();
        }

        public void Disable()
        {
            _gamePauseService.Paused += OnPauseStarted;
            _gamePauseService.Resumed += OnPauseEnded;

            _settingsModel.MuteChanged += OnMuteChanged;
            _settingsModel.MasterVolumeChanged += OnMasterVolumeChanged;
            _settingsModel.MusicVolumeChanged += OnMusicVolumeChanged;
            _settingsModel.SfxVolumeChanged += OnSfxVolumeChanged;
        }

        public void BindListenerTo(GameObject gameObject)
        {
            Object.FindObjectOfType<StudioListener>().BindAttenuation(gameObject);
        }

        private void OnPauseStarted()
        {
            RuntimeManager.PauseAllEvents(true);
            RuntimeManager.CoreSystem.mixerSuspend();

            AudioListener.pause = true;
            AudioListener.volume = 0;
        }

        private void OnPauseEnded()
        {
            RuntimeManager.PauseAllEvents(false);
            RuntimeManager.CoreSystem.mixerResume();

            AudioListener.pause = false;
            AudioListener.volume = 1;
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

        private void OnMuteChanged(bool isMuted)
        {
            _masterBus.setMute(isMuted);
            _musicBus.setMute(isMuted);
            _sfxBus.setMute(isMuted);
        }

        private void OnMasterVolumeChanged(int volume)
        {
            _masterBus.setVolume(volume / 100f);
        }

        private void OnMusicVolumeChanged(int volume)
        {
            _musicBus.setVolume(volume / 100f);
        }

        private void OnSfxVolumeChanged(int volume)
        {
            _sfxBus.setVolume(volume / 100f);
        }

        private void ApplySettings()
        {
            OnMuteChanged(_settingsModel.IsMuted);
            OnMasterVolumeChanged(_settingsModel.MasterVolume);
            OnMusicVolumeChanged(_settingsModel.MusicVolume);
            OnSfxVolumeChanged(_settingsModel.VfxVolume);
        }
    }
}