using CodeBase.Infrastructure.Builders;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Presentation.ViewModels;
using CodeBase.Presentation.Views;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class GameLoopViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IViewModelProvider _viewModelProvider;
        private readonly UpgradeDescriptionBuilder _upgradeDescriptionBuilder;

        public GameLoopViewFactory(
            IConfigurationProvider configurationProvider,
            IViewModelProvider viewModelProvider,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _configurationProvider = configurationProvider;
            _viewModelProvider = viewModelProvider;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;
        }

        public GameLoopView CreateGameLoopView()
        {
            GameLoopView gameLoopView = GameObject.Instantiate(_configurationProvider.GameLoopConfig.GameLoopView);
            gameLoopView.Initialize(
                _viewModelProvider.Get<GameLoopViewModel>(),
                _viewModelProvider.Get<LevelUpViewModel>(),
                _upgradeDescriptionBuilder);

            return gameLoopView;
        }
    }
}