using System;

namespace GameCore.Source.Domain.Models
{
    public class SettingsModel
    {
        public event Action<int> MasterVolumeChanged;
        public event Action<int> MusicVolumeChanged;
        public event Action<int> SfxVolumeChanged;
        public event Action<bool> MuteChanged;
        public int MasterVolume { get; private set; }
        public int MusicVolume { get; private set; }
        public int SfxVolume { get; private set; }
        public bool IsMuted { get; private set; }

        public void SetMasterVolume(int volume)
        {
            MasterVolume = volume;
            MasterVolumeChanged?.Invoke(volume);
        }

        public void SetMusicVolume(int volume)
        {
            MusicVolume = volume;
            MusicVolumeChanged?.Invoke(volume);
        }

        public void SetSfxVolume(int volume)
        {
            SfxVolume = volume;
            SfxVolumeChanged?.Invoke(volume);
        }

        public void Mute()
        {
            IsMuted = true;
            MuteChanged?.Invoke(IsMuted);
        }

        public void UnMute()
        {
            IsMuted = false;
            MuteChanged?.Invoke(IsMuted);
        }

        public void SetMute(bool muted)
        {
            if (muted)
                Mute();
            else
                UnMute();
        }
    }
}