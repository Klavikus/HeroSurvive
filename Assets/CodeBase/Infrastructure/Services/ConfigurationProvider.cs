using CodeBase.Configs;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Infrastructure.Services
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly ConfigurationContainer _configurationContainer;

        public ConfigurationProvider(ConfigurationContainer configurationContainer) =>
            _configurationContainer = configurationContainer;

        public BasePropertiesConfigSO GetBasePropertiesConfig() => _configurationContainer.BasePropertiesConfigSO;
        public MainMenuConfigurationSO GetMainMenuConfig() => _configurationContainer.MainMenuConfigurationSo;
        public HeroesConfigSO GetHeroConfig() => _configurationContainer.HeroConfigSO;
        public UpgradesConfigSO GetUpgradesConfig() => _configurationContainer.UpgradesConfigSO;
        public ColorConfigSO GetColorsConfig() => _configurationContainer.ColorConfigSO;
        public GameLoopConfigSO GetGameLoopConfig() => _configurationContainer.GameLoopConfigSO;
        public EnemyConfigSO GetEnemyConfig() => _configurationContainer.EnemyConfigSO;
        public StageCompetitionConfigSO GetStageCompetitionConfig() => _configurationContainer.StageCompetitionConfigSO;
        public AbilityConfigSO[] GetAbilityConfigs() => _configurationContainer.AbilityConfigsSO;

        public CoroutineRunner GetCoroutineRunner() => _configurationContainer.CoroutineRunner;
    }
}