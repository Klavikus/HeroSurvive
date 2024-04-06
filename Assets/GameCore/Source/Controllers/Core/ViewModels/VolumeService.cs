using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.ViewModels
{
    public class VolumeService
    {
        private readonly SettingsModel _settingsModel;


        public VolumeService(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;

            _settingsModel.MasterVolumeChanged += OnMasterVolumeChanged;
            _settingsModel.MusicVolumeChanged += OnMusicVolumeChanged;
            _settingsModel.SfxVolumeChanged += OnSfxVolumeChanged;
            _settingsModel.MuteChanged += OnMuteChanged;
        }

        ~VolumeService()
        {
            _settingsModel.MasterVolumeChanged -= OnMasterVolumeChanged;
            _settingsModel.MusicVolumeChanged -= OnMusicVolumeChanged;
            _settingsModel.SfxVolumeChanged -= OnSfxVolumeChanged;
            _settingsModel.MuteChanged -= OnMuteChanged;
        }

        private void OnMasterVolumeChanged(int volume)
        {
            // _masterBus.setVolume(volume / 100f);
        }

        private void OnMusicVolumeChanged(int volume)
        {
            // _musicBus.setVolume(volume / 100f);
        }

        private void OnSfxVolumeChanged(int volume)
        {
            // _sfxBus.setVolume(volume / 100f);
        }

        private void OnMuteChanged(bool muteStatus)
        {
            // _masterBus.setMute(muteStatus);
            // _sfxBus.setMute(muteStatus);
            // _musicBus.setMute(muteStatus);
        }
    }
}