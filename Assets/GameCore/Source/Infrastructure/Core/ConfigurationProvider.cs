using System.Collections.Generic;
using GameCore.Source.Domain.Configs;

namespace GameCore.Source.Domain.Services
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly ConfigurationContainer _configurationContainer;

        public ConfigurationProvider(ConfigurationContainer configurationContainer)
        {
            _configurationContainer = configurationContainer;
        }

        public VfxConfig VfxConfig => _configurationContainer.VfxConfig;
        public AbilityConfigSO[] AbilityConfigs => _configurationContainer.AbilityConfigsSO;
        public BasePropertiesConfigSO BasePropertiesConfig => _configurationContainer.BasePropertiesConfigSO;
        public GameLoopConfigSO GameLoopConfig => _configurationContainer.GameLoopConfigSO;
        public StageCompetitionConfigSO StageCompetitionConfig => _configurationContainer.StageCompetitionConfigSO;
        public EnemyConfigSO EnemyConfig => _configurationContainer.EnemyConfigSO;
        public UpgradesConfigSO UpgradesConfig => _configurationContainer.UpgradesConfigSO;
        public HeroesConfigSO HeroConfig => _configurationContainer.HeroConfigSO;
    }
}