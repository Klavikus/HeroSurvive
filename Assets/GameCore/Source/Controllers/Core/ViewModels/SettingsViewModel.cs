using System;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.ViewModels
{
    public class SettingsViewModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly VolumeService _volumeService;

        public SettingsViewModel(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
            _volumeService = new VolumeService(settingsModel);
        }

        public float GetMasterVolume() => _settingsModel.MasterVolume;
        public float GetMusicVolume() => _settingsModel.MusicVolume;
        public float GetSfxVolume() => _settingsModel.SfxVolume;
        public bool GetMuteStatus() => _settingsModel.IsMuted;

        public void SetMasterVolume(float volume) => _settingsModel.SetMasterVolume((int) volume);
        public void SetMusicVolume(float volume) => _settingsModel.SetMusicVolume((int) volume);
        public void SetSfxVolume(float volume) => _settingsModel.SetSfxVolume((int) volume);
        public void SetMuteStatus(bool value) => _settingsModel.SetMute(value);
    }
}