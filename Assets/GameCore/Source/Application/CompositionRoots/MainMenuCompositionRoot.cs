using System;
using System.Collections.Generic;
using CodeBase.GameCore.Presentation.ViewModels;
using CodeBase.GameCore.Presentation.Views.Upgrades;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Core.GameLoop;
using GameCore.Source.Presentation.Core.MainMenu;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.Common.WindowFsm.Runtime.Implementation;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private MVPLeaderBoardsView _leaderBoardsView;
        [SerializeField] private LocalizationSystemView _localizationSystemView;
        [SerializeField] private UpgradesSelectorView _upgradesSelectorView;

        public override async void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(MainMenuWindow)] = new MainMenuWindow(),
                [typeof(LeaderBoardWindow)] = new LeaderBoardWindow(),
                [typeof(UpgradeSelectorWindow)] = new UpgradeSelectorWindow(),
            };

            WindowFsm<MainMenuWindow> windowFsm = new WindowFsm<MainMenuWindow>(windows);

            IConfigurationProvider configurationProvider = serviceContainer.Single<IConfigurationProvider>();
            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            ILocalizationService localizationService = serviceContainer.Single<ILocalizationService>();

            IModelProvider modelProvider = serviceContainer.Single<IModelProvider>();
            IAudioPlayerService audioPlayerService = serviceContainer.Single<IAudioPlayerService>();

            UpgradeModel[] upgradeModels = modelProvider.Get<UpgradeModel[]>();
            CurrencyModel currencyModel = modelProvider.Get<CurrencyModel>();

            IUpgradeService upgradeService = new UpgradeService(upgradeModels);

            PersistentUpgradeService persistentUpgradeService = new PersistentUpgradeService(
                upgradeModels,
                currencyModel,
                upgradeService,
                audioPlayerService
            );

            LocalizationSystemPresenter localizationSystemPresenter = new(_localizationSystemView, localizationService);
            _localizationSystemView.Construct(localizationSystemPresenter);

            MainMenuPresenter mainMenuPresenter = new(windowFsm, _mainMenuView, gameStateMachine);
            _mainMenuView.Construct(mainMenuPresenter);

            LeaderBoardPresenter leaderBoardPresenter = new(_leaderBoardsView, windowFsm);
            _leaderBoardsView.Construct(leaderBoardPresenter);

            UpgradeSelectorPresenter upgradeSelectorPresenter = new(
                windowFsm,
                _upgradesSelectorView,
                persistentUpgradeService);
            _upgradesSelectorView.Construct(upgradeSelectorPresenter);
        }
    }
}