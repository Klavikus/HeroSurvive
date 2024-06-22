namespace GameCore.Source.Controllers.Core.ViewModels
{
    public interface ISettingsViewModel
    {
        float GetMasterVolume();
        float GetMusicVolume();
        float GetSfxVolume();
        bool GetMuteStatus();
        void SetMasterVolume(float volume);
        void SetMusicVolume(float volume);
        void SetVfxVolume(float volume);
        void SetMuteStatus(bool value);
    }
}