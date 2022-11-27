using System.Collections.Generic;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;
using CodeBase.MVVM.Views.HeroSelector;
using CodeBase.MVVM.Views.Upgrades;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly ConfigurationProvider _configurationProvider;
        private readonly HeroSelectorViewModel _heroSelectorViewModel;
        private readonly MainPropertiesViewModel _mainPropertiesViewModel;
        private readonly MenuViewModel _menuViewModel;
        private readonly UpgradeViewModel _upgradeViewModel;
        private readonly UpgradeDescriptionBuilder _descriptionBuilder;
        private readonly UserNameViewModel _userNameViewModel;
        private readonly CurrencyViewModel _currencyViewModel;
        private readonly GameLoopViewModel _gameLoopViewModel;

        public ViewFactory(ConfigurationProvider configurationProvider,
            HeroSelectorViewModel heroSelectorViewModel,
            MainPropertiesViewModel mainPropertiesViewModel,
            MenuViewModel menuViewModel,
            UpgradeViewModel upgradeViewModel,
            CurrencyViewModel currencyViewModel,
            UpgradeDescriptionBuilder descriptionBuilder,
            UserNameViewModel userNameViewModel)
        {
            _configurationProvider = configurationProvider;
            _heroSelectorViewModel = heroSelectorViewModel;
            _mainPropertiesViewModel = mainPropertiesViewModel;
            _menuViewModel = menuViewModel;
            _upgradeViewModel = upgradeViewModel;
            _currencyViewModel = currencyViewModel;
            _descriptionBuilder = descriptionBuilder;
            _userNameViewModel = userNameViewModel;
        }

        public HeroSelectorView CreateHeroSelectorView()
        {
            HeroSelectorView heroSelectorView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.HeroSelectorView);
            heroSelectorView.Initialize(_heroSelectorViewModel, _currencyViewModel, _descriptionBuilder);
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
                heroView.Initialize(heroData, _heroSelectorViewModel);
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
                result[i].Initialize(_mainPropertiesViewModel, viewData[i], _descriptionBuilder);
            }

            return result;
        }

        public StartMenuView CreateStartMenu()
        {
            StartMenuView startMenuView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.StartMenuView);
            startMenuView.Initialize(_menuViewModel);
            return startMenuView;
        }

        public UpgradesSelectorView CreateUpgradesSelectorView()
        {
            UpgradesSelectorView upgradesSelectorView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradesSelectorView);
            upgradesSelectorView.Initialize(
                _menuViewModel,
                _upgradeViewModel,
                _currencyViewModel,
                viewFactory: this,
                _descriptionBuilder);
            return upgradesSelectorView;
        }

        private UpgradeView CreateUpgradeView(UpgradeData upgradeData)
        {
            UpgradeView upgradeView = GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradeView);

            upgradeView.Initialize(_upgradeViewModel, upgradeData,
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

            currencyView.Initialize(_currencyViewModel, _descriptionBuilder);
            return currencyView;
        }

        public UserNameSetterView CreateUserNameView()
        {
            UserNameSetterView userNameSetterView = GameObject.Instantiate(_configurationProvider.UserNameSetterView);

            userNameSetterView.Initialize(_userNameViewModel);
            return userNameSetterView;
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
    }

    public class GameLoopViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly GameLoopViewModel _gameLoopViewModel;
        private readonly LevelUpViewModel _levelUpViewModel;
        private readonly UpgradeDescriptionBuilder _upgradeDescriptionBuilder;

        public GameLoopViewFactory(IConfigurationProvider configurationProvider,
            GameLoopViewModel gameLoopViewModel,
            LevelUpViewModel levelUpViewModel,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _configurationProvider = configurationProvider;
            _gameLoopViewModel = gameLoopViewModel;
            _levelUpViewModel = levelUpViewModel;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;
        }

        public GameLoopView CreateGameLoopView()
        {
            GameLoopView gameLoopView = GameObject.Instantiate(_configurationProvider.GameLoopConfig.GameLoopView);
            gameLoopView.Initialize(_gameLoopViewModel, _levelUpViewModel, _upgradeDescriptionBuilder);

            return gameLoopView;
        }
    }
}