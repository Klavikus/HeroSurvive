using GameCore.Source.Controllers.Api;
using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class SelectableHeroPresenter : IPresenter
    {
        private readonly ISelectableHeroView _view;
        private readonly IHeroSelectorViewModel _viewModel;
        private readonly HeroData _heroData;

        public SelectableHeroPresenter(
            ISelectableHeroView view,
            IHeroSelectorViewModel viewModel,
            HeroData heroData
        )
        {
            _view = view;
            _viewModel = viewModel;
            _heroData = heroData;
        }

        public void Enable()
        {
            _view.Image.sprite = _heroData.Sprite;
            _view.SelectionBorder.enabled = _viewModel.CurrentSelectedHeroData == _heroData;

            _view.Clicked += OnClicked;
            _viewModel.HeroSelected += OnHeroSelected;
        }

        public void Disable()
        {
            _view.Clicked -= OnClicked;
            _viewModel.HeroSelected -= OnHeroSelected;
        }

        private void OnHeroSelected(HeroData heroData) =>
            _view.SelectionBorder.enabled = _heroData == heroData;

        private void OnClicked() =>
            _viewModel.SelectHero(_heroData);
    }
}