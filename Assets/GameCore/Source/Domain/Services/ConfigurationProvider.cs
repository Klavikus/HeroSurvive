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

        public VfxConfig VfxConfig { get; }
        public List<AbilityConfigSO> AbilityConfigs { get; }
    }
}