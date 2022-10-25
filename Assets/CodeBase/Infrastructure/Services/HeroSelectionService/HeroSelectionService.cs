using System;
using CodeBase.HeroSelection;

namespace CodeBase.Infrastructure.Services.HeroSelectionService
{
    class HeroSelectionService : IHeroSelectionService
    {
        private readonly IConfigurationProvider _configurationProvider;
        
        private HeroData _currentSelectedHero;
        private MainProperties _mainProperties;
        private HeroViewController _heroViewController;
        
        public event Action HeroSelected;

        public HeroSelectionService(IConfigurationProvider configurationProvider, HeroViewController heroViewController)
        {
            _configurationProvider = configurationProvider;

            _heroViewController = heroViewController;
            _heroViewController.HeroSelected += OnHeroSelected;
        }

        private void OnHeroSelected(HeroData heroData)
        {
            _currentSelectedHero = heroData;
            UpdateMainProperties();
            HeroSelected?.Invoke();
        }

        public MainProperties GetHeroPropertiesData() => _mainProperties;

        private void UpdateMainProperties()
        {
            _mainProperties = new MainProperties();
            foreach (AdditionalHeroProperty heroProperty in _currentSelectedHero.AdditionalProperties)
                _mainProperties.UpdateProperty(heroProperty.BaseProperty, heroProperty.Value);
        }
    }
}