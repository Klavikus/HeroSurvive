using CodeBase.Configs;
using CodeBase.Domain;

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
    }
}