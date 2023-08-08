using CodeBase.Domain;
using FMOD.Studio;
using FMODUnity;

namespace CodeBase.Presentation
{
    public class VolumeService
    {
        private readonly SettingsModel _settingsModel;

        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _sfxBus;

        public VolumeService(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;

            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");

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

        private void OnMasterVolumeChanged(int volume) => _masterBus.setVolume(volume / 100f);
        private void OnMusicVolumeChanged(int volume) => _musicBus.setVolume(volume / 100f);
        private void OnSfxVolumeChanged(int volume) => _sfxBus.setVolume(volume / 100f);
        private void OnMuteChanged(bool muteStatus)
        {
            _masterBus.setMute(muteStatus);
            _sfxBus.setMute(muteStatus);
            _musicBus.setMute(muteStatus);
        }
    }
}