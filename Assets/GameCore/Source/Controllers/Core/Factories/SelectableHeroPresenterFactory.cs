using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.ViewModels;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Presenters.MainMenu;
using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class SelectableHeroPresenterFactory : ISelectableHeroPresenterFactory
    {
        private readonly IHeroSelectorViewModel _heroSelectorViewModel;

        public SelectableHeroPresenterFactory(IHeroSelectorViewModel heroSelectorViewModel)
        {
            _heroSelectorViewModel = heroSelectorViewModel;
        }

        public IPresenter Create(ISelectableHeroView view, HeroData heroData) =>
            new SelectableHeroPresenter(view, _heroSelectorViewModel, heroData);
    }
}