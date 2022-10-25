using CodeBase.HeroSelection;
using CodeBase.Infrastructure.Factories;

namespace CodeBase.Infrastructure.Services
{
    public interface IConfigurationProvider : IService
    {
        BasePropertiesConfigSO GetBasePropertiesConfig();
        MainMenuConfiguration GetMainMenuConfig();
        HeroesConfigSO GetHeroConfig();
    }
}