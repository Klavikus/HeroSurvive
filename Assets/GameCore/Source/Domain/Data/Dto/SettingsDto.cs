using Modules.DAL.Abstract.Data;

namespace GameCore.Source.Domain.Data.Dto
{
    public class SettingsDto : IEntity
    {
        public static string DefaultId = nameof(SettingsDto);

        public int MasterVolume;
        public int MusicVolume;
        public int VfxVolume;
        public bool IsMuted;

        public object Clone()
        {
            return new SettingsDto()
            {
                MasterVolume = MasterVolume,
                MusicVolume = MusicVolume,
                VfxVolume = VfxVolume,
                IsMuted = IsMuted,
            };
        }

        public bool Equals(IEntity other) =>
            other?.Id == Id;

        public string Id => DefaultId;
    }
}