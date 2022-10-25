using System;
using CodeBase.HeroSelection;

namespace CodeBase.Infrastructure.Services.HeroSelectionService
{
    public interface IHeroSelectionService : IService
    {
        event Action HeroSelected;
        MainProperties GetHeroPropertiesData();
    }
}