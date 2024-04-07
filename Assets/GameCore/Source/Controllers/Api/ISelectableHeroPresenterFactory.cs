using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.GameLoop;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Api
{
    public interface ISelectableHeroPresenterFactory
    {
        IPresenter Create(ISelectableHeroView view, HeroData heroData);
    }
}