using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Api.Factories
{
    public interface ISelectableHeroPresenterFactory
    {
        IPresenter Create(ISelectableHeroView view, HeroData heroData);
    }
}