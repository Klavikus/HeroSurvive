using System;
using CodeBase.Domain.Models;

namespace CodeBase.Presentation.ViewModels
{
    public class SettingsViewModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly MenuModel _menuModel;
        private readonly VolumeService _volumeService;

        public SettingsViewModel(SettingsModel settingsModel, MenuModel menuModel)
        {
            _settingsModel = settingsModel;
            _menuModel = menuModel;
            _volumeService = new VolumeService(settingsModel);

            _menuModel.InvokedSettingsShow += () => InvokedShow?.Invoke();
            _menuModel.InvokedSettingsHide += () => InvokedHide?.Invoke();
        }

        public event Action InvokedShow;
        public event Action InvokedHide;

        public void Show() => _menuModel.InvokeSettingsShow();
        public void Hide() => _menuModel.InvokeSettingsHide();
        
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