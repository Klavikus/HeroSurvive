using GameCore.Source.Domain.Configs;
using UnityEngine;

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
        string LocalizationTablePath { get; }
        string BaseLanguage { get; }
        ColorConfigSO ColorsConfig { get; }
        GameObject UpgradeLevelView { get; }
        GameObject PersistentUpgradeView { get; }
        GameObject SelectableHeroView { get; }
    }
}