using System.Collections;
using CodeBase.Configs;
using CodeBase.Infrastructure.Factories;

namespace CodeBase.Infrastructure.Services
{
    public interface IConfigurationProvider : IService
    {
        BasePropertiesConfigSO GetBasePropertiesConfig();
        MainMenuConfigurationSO GetMainMenuConfig();
        HeroesConfigSO GetHeroConfig();
        StageCompetitionConfigSO GetStageCompetitionConfig();
        GameLoopConfigSO GetGameLoopConfig();
        AbilityConfigSO[] GetAbilityConfigs();
    }
}