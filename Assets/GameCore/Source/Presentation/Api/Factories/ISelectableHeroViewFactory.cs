using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;

namespace GameCore.Source.Presentation.Api.Factories
{
    public interface ISelectableHeroViewFactory
    {
        ISelectableHeroView[] Create();
    }
}