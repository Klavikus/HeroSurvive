using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Handlers;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Api.ViewModels;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Presenters.GameLoop;
using GameCore.Source.Controllers.Core.Presenters.MainMenu;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Controllers.Core.ViewModels;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Core;
using GameCore.Source.Presentation.Core.GameLoop;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.Common.WindowFsm.Runtime.Implementation;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public class GameLoopCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private GameLoopView _gameLoopView;
        [SerializeField] private DeathView _deathView;
        [SerializeField] private WinView _winView;
        [SerializeField] private LevelUpSystemView _levelUpSystemView;
        [SerializeField] private LocalizationSystemView _localizationSystemView;
        [SerializeField] private SettingsView _settingsView;
        [SerializeField] private PauseView _pauseView;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            IWindowFsm windowFsm = CreateWindowFsm();

            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            IConfigurationProvider configurationProvider = serviceContainer.Single<IConfigurationProvider>();
            IAudioPlayerService audioPlayerService = serviceContainer.Single<IAudioPlayerService>();
            IVfxService vfxService = serviceContainer.Single<IVfxService>();
            ICoroutineRunner coroutineRunner = serviceContainer.Single<ICoroutineRunner>();
            IGamePauseService gamePauseService = serviceContainer.Single<IGamePauseService>();
            ILocalizationService localizationService = serviceContainer.Single<ILocalizationService>();
            IModelProvider modelProvider = serviceContainer.Single<IModelProvider>();
            IPropertyProvider propertyProvider = serviceContainer.Single<IPropertyProvider>();
            IGameLoopService gameLoopService = serviceContainer.Single<IGameLoopService>();
            IAdsProvider adsProvider = serviceContainer.Single<IAdsProvider>();
            IUpgradeDescriptionBuilder upgradeDescriptionBuilder =
                serviceContainer.Single<IUpgradeDescriptionBuilder>();
            IProgressService progressService = serviceContainer.Single<IProgressService>();
            IApplicationFocusChangeHandler focusChangeHandler =
                serviceContainer.Single<IApplicationFocusChangeHandler>();
            
            HeroModel heroModel = modelProvider.Get<HeroModel>();
            PlayerModel playerModel = modelProvider.Get<PlayerModel>();

            IAbilityUpgradeService abilityUpgradeService = new AbilityUpgradeService(configurationProvider);
            CurrencyModel currencyModel = modelProvider.Get<CurrencyModel>();

            LevelUpModel levelUpModel = new(abilityUpgradeService, currencyModel);

            ILevelUpViewModel levelUpViewModel = new LevelUpViewModel(levelUpModel, abilityUpgradeService, adsProvider);

            SettingsModel settingsModel = modelProvider.Get<SettingsModel>();
            SettingsViewModel settingsViewModel = new SettingsViewModel(settingsModel, progressService);

            EnemyFactory enemyFactory = new(configurationProvider, vfxService, audioPlayerService, gameLoopService);

            TargetService targetService = new(enemyFactory, playerModel);
            AbilityProjectionBuilder abilityProjectionBuilder = new(
                targetService,
                audioPlayerService,
                coroutineRunner);
            AbilityFactory abilityFactory = new(abilityProjectionBuilder, coroutineRunner);

            HealthViewBuilder healthViewBuilder = new(coroutineRunner);

            PlayerFactory playerFactory = new(
                heroModel,
                propertyProvider,
                abilityUpgradeService,
                abilityFactory,
                audioPlayerService,
                playerModel,
                healthViewBuilder);

            IEnemySpawnService enemySpawnService = new EnemySpawnService(targetService, enemyFactory);

            ILeveCompetitionService levelCompetitionService = new LeveCompetitionService(
                enemySpawnService,
                configurationProvider,
                modelProvider,
                vfxService,
                levelUpModel);

            vfxService.Reset();

            ConstructViews(
                levelUpModel,
                localizationService,
                windowFsm,
                gameStateMachine,
                gameLoopService,
                gamePauseService,
                levelCompetitionService,
                currencyModel,
                playerFactory,
                audioPlayerService,
                healthViewBuilder,
                levelUpViewModel,
                upgradeDescriptionBuilder,
                settingsViewModel,
                adsProvider);

            // TODO: Fix this
            gamePauseService.Paused += () =>
            {
                if (gamePauseService.IsInvokeByUI)
                    return;

                windowFsm.OpenWindow<PauseWindow>();
            };

            focusChangeHandler.FocusDropped += gamePauseService.InvokeByFocusChanging;
        }

        private WindowFsm<GameLoopWindow> CreateWindowFsm()
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(GameLoopWindow)] = new GameLoopWindow(),
                [typeof(DeathWindow)] = new DeathWindow(),
                [typeof(WinWindow)] = new WinWindow(),
                [typeof(LevelUpWindow)] = new LevelUpWindow(),
                [typeof(SettingsWindow)] = new SettingsWindow(),
                [typeof(PauseWindow)] = new PauseWindow(),
            };

            WindowFsm<GameLoopWindow> windowFsm = new WindowFsm<GameLoopWindow>(windows);

            return windowFsm;
        }

        private void ConstructViews(LevelUpModel levelUpModel,
            ILocalizationService localizationService,
            IWindowFsm windowFsm,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService,
            IGamePauseService gamePauseService,
            ILeveCompetitionService levelCompetitionService,
            CurrencyModel currencyModel,
            PlayerFactory playerFactory,
            IAudioPlayerService audioPlayerService,
            HealthViewBuilder healthViewBuilder,
            ILevelUpViewModel levelUpViewModel,
            IUpgradeDescriptionBuilder upgradeDescriptionBuilder,
            SettingsViewModel settingsViewModel,
            IAdsProvider adsProvider)
        {
            LevelUpSystemPresenter levelUpSystemPresenter =
                new(windowFsm, _levelUpSystemView, levelUpViewModel, gamePauseService, localizationService,
                    upgradeDescriptionBuilder);

            LocalizationSystemPresenter localizationSystemPresenter = new(_localizationSystemView, localizationService);

            DeathPresenter deathPresenter = new(
                windowFsm,
                _deathView,
                gameStateMachine,
                gameLoopService,
                gamePauseService,
                adsProvider);

            WinPresenter winPresenter = new(
                windowFsm,
                _winView,
                gameStateMachine,
                gameLoopService,
                gamePauseService,
                levelCompetitionService,
                currencyModel);

            GameLoopPresenter gameLoopPresenter = new(
                windowFsm,
                _gameLoopView,
                gameStateMachine,
                gameLoopService,
                playerFactory,
                levelCompetitionService,
                audioPlayerService,
                healthViewBuilder,
                currencyModel,
                levelUpModel);

            SettingsPresenter settingsPresenter = new(windowFsm, _settingsView, settingsViewModel, gamePauseService);

            PausePresenter pausePresenter = new(windowFsm, _pauseView, gameStateMachine, gamePauseService);

            _localizationSystemView.Construct(localizationSystemPresenter);
            _levelUpSystemView.Construct(levelUpSystemPresenter);
            _deathView.Construct(deathPresenter);
            _winView.Construct(winPresenter);
            _gameLoopView.Construct(gameLoopPresenter);
            _settingsView.Construct(settingsPresenter);
            _pauseView.Construct(pausePresenter);
        }
    }
}