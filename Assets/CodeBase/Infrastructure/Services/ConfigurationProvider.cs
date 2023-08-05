using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly ConfigurationContainer _configurationContainer;

        public ConfigurationProvider(ConfigurationContainer configurationContainer) =>
            _configurationContainer = configurationContainer;

        public BasePropertiesConfigSO BasePropertiesConfig => _configurationContainer.BasePropertiesConfigSO;
        public MainMenuConfigurationSO MainMenuConfig => _configurationContainer.MainMenuConfigurationSo;
        public HeroesConfigSO HeroConfig => _configurationContainer.HeroConfigSO;
        public UpgradesConfigSO UpgradesConfig => _configurationContainer.UpgradesConfigSO;
        public ColorConfigSO ColorsConfig => _configurationContainer.ColorConfigSO;
        public GameLoopConfigSO GameLoopConfig => _configurationContainer.GameLoopConfigSO;
        public EnemyConfigSO EnemyConfig => _configurationContainer.EnemyConfigSO;
        public VfxConfig VfxConfig => _configurationContainer.VfxConfig;
        public StageCompetitionConfigSO StageCompetitionConfig => _configurationContainer.StageCompetitionConfigSO;
        public AbilityConfigSO[] AbilityConfigs => _configurationContainer.AbilityConfigsSO;
    }
}