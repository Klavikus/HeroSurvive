using System.Collections.Generic;
using CodeBase.Domain;
using CodeBase.Presentation;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class ViewFactory : IViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly UpgradeDescriptionBuilder _descriptionBuilder;
        private readonly IProvider _provider;

        public ViewFactory(
            IConfigurationProvider configurationProvider,
            IProvider provider,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _configurationProvider = configurationProvider;
            _provider = provider;
            _descriptionBuilder = upgradeDescriptionBuilder;
        }

        public HeroSelectorView CreateHeroSelectorView()
        {
            HeroSelectorView heroSelectorView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.HeroSelectorView);
            heroSelectorView.Initialize(
                _provider.Get<HeroSelectorViewModel>(),
                _provider.Get<CurrencyViewModel>(),
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
                heroView.Initialize(heroData, _provider.Get<HeroSelectorViewModel>());
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
                result[i].Initialize(_provider.Get<MainPropertiesViewModel>(), viewData[i],
                    _descriptionBuilder);
            }

            return result;
        }

        public StartMenuView CreateStartMenu()
        {
            StartMenuView startMenuView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.StartMenuView);
            startMenuView.Initialize(_provider.Get<MenuViewModel>());
            return startMenuView;
        }

        public UpgradesSelectorView CreateUpgradesSelectorView()
        {
            UpgradesSelectorView upgradesSelectorView =
                GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradesSelectorView);
            upgradesSelectorView.Initialize(
                _provider.Get<MenuViewModel>(),
                _provider.Get<UpgradeViewModel>(),
                _provider.Get<CurrencyViewModel>(),
                viewFactory: this,
                _descriptionBuilder);
            return upgradesSelectorView;
        }

        private UpgradeView CreateUpgradeView(UpgradeData upgradeData)
        {
            UpgradeView upgradeView = GameObject.Instantiate(_configurationProvider.MainMenuConfig.UpgradeView);

            upgradeView.Initialize(_provider.Get<UpgradeViewModel>(), upgradeData,
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

            currencyView.Initialize(_provider.Get<CurrencyViewModel>(), _descriptionBuilder);
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
    }
}