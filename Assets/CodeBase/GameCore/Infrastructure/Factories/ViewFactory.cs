using System.Collections.Generic;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Infrastructure.Builders;
using CodeBase.GameCore.Infrastructure.Services;
using CodeBase.GameCore.Presentation.ViewModels;
using CodeBase.GameCore.Presentation.Views;
using CodeBase.GameCore.Presentation.Views.HeroSelector;
using CodeBase.GameCore.Presentation.Views.Upgrades;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly UpgradeDescriptionBuilder _descriptionBuilder;
        private readonly IProvider _viewModelProvider;

        public ViewFactory(
            IConfigurationProvider configurationProvider,
            IProvider viewModelProvider,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _configurationProvider = configurationProvider;
            _viewModelProvider = viewModelProvider;
            _descriptionBuilder = upgradeDescriptionBuilder;
        }

        public HeroSelectorView CreateHeroSelectorView()
        {
            HeroSelectorView heroSelectorView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.HeroSelectorView);
            heroSelectorView.Initialize(
                _viewModelProvider.Get<HeroSelectorViewModel>(),
                _viewModelProvider.Get<CurrencyViewModel>(),
                _descriptionBuilder);
            return heroSelectorView;
        }

        public HeroView[] CreateHeroViews()
        {
            HeroData[] heroesData = _configurationProvider.HeroConfig.HeroesData;
            HeroView[] result = new HeroView[heroesData.Length];

            for (var i = 0; i < heroesData.Length; i++)
            {
                HeroData heroData = heroesData[i];
                HeroView heroView = GameObject.Instantiate(_configurationProvider.HeroConfig.BaseHeroView);
                heroView.Initialize(heroData, _viewModelProvider.Get<HeroSelectorViewModel>());
                result[i] = heroView;
            }

            return result;
        }

        public PropertyView[] CreatePropertyViews()
        {
            IReadOnlyList<MainPropertyViewData> viewData =
                _configurationProvider.BasePropertiesConfig.PropertiesData;
            PropertyView[] result = new PropertyView[viewData.Count];

            for (int i = 0; i < viewData.Count; i++)
            {
                result[i] = GameObject.Instantiate(_configurationProvider.BasePropertiesConfig.PropertyView);
                result[i].Initialize(_viewModelProvider.Get<MainPropertiesViewModel>(), viewData[i],
                    _descriptionBuilder);
            }

            return result;
        }

        public StartMenuView CreateStartMenu()
        {
            StartMenuView startMenuView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.StartMenuView);
            startMenuView.Initialize(_viewModelProvider.Get<MenuViewModel>());
            return startMenuView;
        }

        public UpgradesSelectorView CreateUpgradesSelectorView()
        {
            UpgradesSelectorView upgradesSelectorView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradesSelectorView);
            upgradesSelectorView.Initialize(
                _viewModelProvider.Get<MenuViewModel>(),
                _viewModelProvider.Get<UpgradeViewModel>(),
                _viewModelProvider.Get<CurrencyViewModel>(),
                viewFactory: this,
                _descriptionBuilder);
            return upgradesSelectorView;
        }

        private UpgradeView CreateUpgradeView(UpgradeData upgradeData)
        {
            UpgradeView upgradeView = GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradeView);

            upgradeView.Initialize(_viewModelProvider.Get<UpgradeViewModel>(), upgradeData,
                CreateUpgradeLevelViews(upgradeData.Upgrades.Length));
            return upgradeView;
        }

        public UpgradeView[] CreateUpgradeViews()
        {
            UpgradeData[] upgradesData = _configurationProvider.UpgradesConfig.UpgradeData;
            UpgradeView[] result = new UpgradeView[upgradesData.Length];

            for (int i = 0; i < upgradesData.Length; i++)
                result[i] = CreateUpgradeView(upgradesData[i]);

            return result;
        }

        public UpgradeLevelView[] CreateUpgradeLevelViews(int count)
        {
            UpgradeLevelView[] result = new UpgradeLevelView[count];

            for (int i = 0; i < count; i++)
                result[i] = GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradeLevelView);

            return result;
        }

        public CurrencyView CreateCurrencyView()
        {
            CurrencyView currencyView = GameObject.Instantiate(_configurationProvider.MainMenuConfig.CurrencyView);

            currencyView.Initialize(_viewModelProvider.Get<CurrencyViewModel>(), _descriptionBuilder);
            return currencyView;
        }

        public LeaderBoardScoreView[] CreateLeaderBoardScoreViews(IReadOnlyList<LeaderBoard> leaderBoards)
        {
            LeaderBoardScoreView[] result = new LeaderBoardScoreView[leaderBoards.Count];

            for (var i = 0; i < leaderBoards.Count; i++)
                result[i] = CreateLeaderBoardScoreView();

            return result;
        }

        public LeaderBoardScoreView CreateLeaderBoardScoreView() =>
            GameObject.Instantiate(_configurationProvider.MainMenuConfig.LeaderBoardScoreView);

        public SettingsView CreateSettingsView()
        {
            SettingsView settingsView = GameObject.Instantiate(_configurationProvider.MainMenuConfig.SettingsView);

            settingsView.Initialize(_viewModelProvider.Get<SettingsViewModel>());

            return settingsView;
        }
    }
}