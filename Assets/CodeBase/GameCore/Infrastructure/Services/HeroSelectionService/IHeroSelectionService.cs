using System;
using CodeBase.GameCore.Domain.Models;

namespace CodeBase.GameCore.Infrastructure.Services.HeroSelectionService
{
    public interface IHeroSelectionService : IService
    {
        event Action HeroSelected;
        MainProperties GetHeroPropertiesData();
    }
}