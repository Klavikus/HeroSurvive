using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;
using FMODUnity;

namespace CodeBase.Infrastructure
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
    }
}