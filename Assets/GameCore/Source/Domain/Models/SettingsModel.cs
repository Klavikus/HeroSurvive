using System;
using GameCore.Source.Domain.Data.Dto;

namespace GameCore.Source.Domain.Models
{
    public class SettingsModel
    {
        public SettingsModel(SettingsDto settingsDto)
        {
            MasterVolume = settingsDto.MasterVolume;
            MusicVolume = settingsDto.MusicVolume;
            VfxVolume = settingsDto.VfxVolume;
            IsMuted = settingsDto.IsMuted;
        }

        public event Action<int> MasterVolumeChanged;
        public event Action<int> MusicVolumeChanged;
        public event Action<int> SfxVolumeChanged;
        public event Action<bool> MuteChanged;
        public int MasterVolume { get; private set; }
        public int MusicVolume { get; private set; }
        public int VfxVolume { get; private set; }
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
            VfxVolume = volume;
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

        public SettingsDto AsDto()
        {
            return new SettingsDto()
            {
                IsMuted = IsMuted,
                MasterVolume = MasterVolume,
                MusicVolume = MusicVolume,
                VfxVolume = VfxVolume
            };
        }
    }
}