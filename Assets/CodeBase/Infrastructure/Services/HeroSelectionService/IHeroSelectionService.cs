using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure.Services.HeroSelectionService
{
    public interface IHeroSelectionService : IService
    {
        event Action HeroSelected;
        MainProperties GetHeroPropertiesData();
    }
}