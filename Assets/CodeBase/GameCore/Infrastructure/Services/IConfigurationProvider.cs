using CodeBase.GameCore.Configs;
using FMODUnity;

namespace CodeBase.GameCore.Infrastructure.Services
{
    public interface IConfigurationProvider : IService
    {
        BasePropertiesConfigSO BasePropertiesConfig { get; }
        MainMenuConfigurationSO MainMenuConfig { get; }
        HeroesConfigSO HeroConfig { get; }
        StageCompetitionConfigSO StageCompetitionConfig { get; }
        GameLoopConfigSO GameLoopConfig { get; }
        AbilityConfigSO[] AbilityConfigs { get; }
        UpgradesConfigSO UpgradesConfig { get; }
        ColorConfigSO ColorsConfig { get; }
        EnemyConfigSO EnemyConfig { get; }
        VfxConfig VfxConfig { get; }
        EventReference FMOD_HitReference { get; }
        EventReference FMOD_UpgradeBuyReference { get; }
        EventReference FMOD_PlayerDiedReference { get; }
        EventReference FMOD_StartLevelReference { get; }
        EventReference FMOD_GameLoopAmbientReference { get; }
        EventReference FMOD_MainMenuAmbientReference { get; }
        EventReference FMOD_Thunder { get; }
    }
}