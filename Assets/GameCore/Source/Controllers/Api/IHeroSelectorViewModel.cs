using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api
{
    public interface IHeroSelectorViewModel
    {
        event Action<HeroData> HeroSelected;
        HeroData CurrentSelectedHeroData { get; }
        int MaxHeroId { get; }
        int CurrentSelectedHeroId { get; }
        void SelectHero(HeroData data);
        void SelectHero(int heroId);
        void Continue();
        void HandleMove(int dX, int dY, int rowCount, int colCount);
    }
}