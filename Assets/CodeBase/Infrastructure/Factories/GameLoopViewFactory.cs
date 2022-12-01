using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
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