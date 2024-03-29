using System;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Presentation.ViewModels;

namespace CodeBase.GameCore.Infrastructure.Services.HeroSelectionService
{
    class HeroSelectionService : IHeroSelectionService
    {
        private HeroData _currentSelectedHero;
        private MainProperties _mainProperties;
        private HeroSelectorViewModel _heroSelectorViewModel;
        public event Action HeroSelected;

        public HeroSelectionService(HeroSelectorViewModel heroSelectorViewModel)
        {
            _heroSelectorViewModel = heroSelectorViewModel;
            _heroSelectorViewModel.HeroSelected += OnHeroSelected;
        }

        public MainProperties GetHeroPropertiesData() => _mainProperties;

        private void OnHeroSelected(HeroData heroData)
        {
            _currentSelectedHero = heroData;
            UpdateMainProperties();
            HeroSelected?.Invoke();
        }

        private void UpdateMainProperties()
        {
            _mainProperties = new MainProperties();
            foreach (AdditionalHeroProperty heroProperty in _currentSelectedHero.AdditionalProperties)
                _mainProperties.UpdateProperty(heroProperty.BaseProperty, heroProperty.Value);
        }
    }
}