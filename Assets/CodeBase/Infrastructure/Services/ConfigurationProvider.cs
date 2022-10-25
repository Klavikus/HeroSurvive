using CodeBase.HeroSelection;
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
        public MainMenuConfiguration GetMainMenuConfig() => _configurationContainer.MainMenuConfiguration;
        public HeroesConfigSO GetHeroConfig() => _configurationContainer.HeroConfigSO;
    }
}