using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;

namespace GameCore.Source.Presentation.Api
{
    public interface IPropertyViewFactory
    {
        IPropertyView[] Create();
    }
}