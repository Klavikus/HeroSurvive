using System;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.Services;

namespace GameCore.Source.Controllers.Core.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly IProgressService _progressService;
        private readonly VolumeService _volumeService;

        public SettingsViewModel(SettingsModel settingsModel, IProgressService progressService)
        {
            _settingsModel = settingsModel ?? throw new ArgumentNullException(nameof(settingsModel));
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
            _volumeService = new VolumeService(settingsModel);
        }

        public float GetMasterVolume() => _settingsModel.MasterVolume;
        public float GetMusicVolume() => _settingsModel.MusicVolume;
        public float GetSfxVolume() => _settingsModel.VfxVolume;
        public bool GetMuteStatus() => _settingsModel.IsMuted;

        public void SetMasterVolume(float volume)
        {
            _settingsModel.SetMasterVolume((int) volume);
            UpdateInProgressRepo();
        }

        public void SetMusicVolume(float volume)
        {
            _settingsModel.SetMusicVolume((int) volume);
            UpdateInProgressRepo();
        }

        public void SetVfxVolume(float volume)
        {
            _settingsModel.SetSfxVolume((int) volume);
            UpdateInProgressRepo();
        }

        public void SetMuteStatus(bool value)
        {
            _settingsModel.SetMute(value);
            UpdateInProgressRepo();
        }

        private void UpdateInProgressRepo() =>
            _progressService.UpdateMasterVolume(_settingsModel.AsDto());
    }
}