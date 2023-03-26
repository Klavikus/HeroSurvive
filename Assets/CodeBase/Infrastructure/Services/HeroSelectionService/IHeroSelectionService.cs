using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
{
    public interface IHeroSelectionService : IService
    {
        event Action HeroSelected;
        MainProperties GetHeroPropertiesData();
    }
}