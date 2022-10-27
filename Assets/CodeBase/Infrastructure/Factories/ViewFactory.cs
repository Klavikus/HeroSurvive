using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class ViewFactory
    {
        private readonly ConfigurationProvider _configurationProvider;
        private readonly HeroSelectorViewModel _heroSelectorViewModel;
        private readonly MainPropertiesViewModel _mainPropertiesViewModel;
        private readonly HeroDescriptionViewModel _heroDescriptionViewModel;
        private readonly BaseAbilityViewModel _baseAbilityViewModel;

        public ViewFactory(ConfigurationProvider configurationProvider,
            HeroSelectorViewModel heroSelectorViewModel,
            MainPropertiesViewModel mainPropertiesViewModel,
            HeroDescriptionViewModel heroDescriptionViewModel, 
            BaseAbilityViewModel baseAbilityViewModel)
        {
            _configurationProvider = configurationProvider;
            _heroSelectorViewModel = heroSelectorViewModel;
            _mainPropertiesViewModel = mainPropertiesViewModel;
            _heroDescriptionViewModel = heroDescriptionViewModel;
            _baseAbilityViewModel = baseAbilityViewModel;
        }

        public HeroSelectorView CreateHeroSelectorView()
        {
            HeroSelectorView heroSelectorView =
                GameObject.Instantiate(_configurationProvider.GetMainMenuConfig().HeroSelectorView);
            return heroSelectorView;
        }

        public HeroView[] CreateHeroViews()
        {
            HeroData[] heroesData = _configurationProvider.GetHeroConfig().HeroesData;
            HeroView[] result = new HeroView[heroesData.Length];

            for (var i = 0; i < heroesData.Length; i++)
            {
                HeroData heroData = heroesData[i];
                HeroView heroView = GameObject.Instantiate(_configurationProvider.GetHeroConfig().BaseHeroView);
                heroView.Initialize(heroData, _heroSelectorViewModel);
                result[i] = heroView;
            }

            return result;
        }

        public PropertyView[] CreatePropertyViews()
        {
            IReadOnlyList<MainPropertyViewData> viewData = _configurationProvider.GetBasePropertiesConfig().PropertiesData;
            PropertyView[] result = new PropertyView[viewData.Count];

            for (int i = 0; i < viewData.Count; i++)
            {
                result[i] = GameObject.Instantiate(_configurationProvider.GetBasePropertiesConfig().PropertyView);
                result[i].Initialize(_mainPropertiesViewModel, viewData[i]);
            }

            return result;
        }

        public HeroDescriptionView CreateHeroDescriptionView()
        {
            HeroDescriptionView heroDescriptionView =
                GameObject.Instantiate(_configurationProvider.GetMainMenuConfig().HeroDescriptionView);
            heroDescriptionView.Initialize(_heroDescriptionViewModel);
            return heroDescriptionView;
        }

        public BaseAbilityView CreateBaseAbilityView()
        {
            BaseAbilityView baseAbilityView =
                GameObject.Instantiate(_configurationProvider.GetMainMenuConfig().BaseAbilityView);
            baseAbilityView.Initialize(_baseAbilityViewModel);
            return baseAbilityView;
        }
    }
}