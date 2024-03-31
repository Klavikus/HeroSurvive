using GameCore.Source.Domain.Configs;

namespace GameCore.Source.Domain.Services
{
    public interface IConfigurationProvider
    {
        VfxConfig VfxConfig { get; }
        AbilityConfigSO[] AbilityConfigs { get; }
        BasePropertiesConfigSO BasePropertiesConfig { get; }
        GameLoopConfigSO GameLoopConfig { get; }
        StageCompetitionConfigSO StageCompetitionConfig { get; }
        EnemyConfigSO EnemyConfig { get; }
        UpgradesConfigSO UpgradesConfig { get; }
        HeroesConfigSO HeroConfig { get; }
    }
}