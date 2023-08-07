using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;
using FMODUnity;

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
        public EventReference FMOD_HitReference => _configurationContainer.FMOD_HitReference;
        public EventReference FMOD_UpgradeBuyReference => _configurationContainer.FMOD_UpgradeBuyReference;
        public EventReference FMOD_PlayerDiedReference => _configurationContainer.FMOD_PlayerDiedReference;
        public StageCompetitionConfigSO StageCompetitionConfig => _configurationContainer.StageCompetitionConfigSO;
        public AbilityConfigSO[] AbilityConfigs => _configurationContainer.AbilityConfigsSO;
    }
}