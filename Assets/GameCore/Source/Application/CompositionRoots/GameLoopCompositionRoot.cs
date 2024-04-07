using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Controllers.Core.Services.PropertiesProviders;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core.Services.DI;
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

        public override void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(GameLoopWindow)] = new GameLoopWindow(),
                [typeof(DeathWindow)] = new DeathWindow(),
                [typeof(WinWindow)] = new WinWindow(),
            };

            WindowFsm<GameLoopWindow> windowFsm = new WindowFsm<GameLoopWindow>(windows);

            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            IConfigurationProvider configurationProvider = serviceContainer.Single<IConfigurationProvider>();
            IAudioPlayerService audioPlayerService = serviceContainer.Single<IAudioPlayerService>();
            IVfxService vfxService = serviceContainer.Single<IVfxService>();
            ICoroutineRunner coroutineRunner = serviceContainer.Single<ICoroutineRunner>();
            IGamePauseService gamePauseService = serviceContainer.Single<IGamePauseService>();
            GameLoopService gameLoopService = new();
            ILocalizationService localizationService = serviceContainer.Single<ILocalizationService>();
            IModelProvider modelProvider = serviceContainer.Single<IModelProvider>();

            UpgradeModel[] upgradeModels = modelProvider.Get<UpgradeModel[]>();
            HeroModel heroModel = modelProvider.Get<HeroModel>();
            PropertiesModel propertiesModel = modelProvider.Get<PropertiesModel>();

            PlayerModel playerModel = modelProvider.Get<PlayerModel>();

            IUpgradeService upgradeService = serviceContainer.Single<IUpgradeService>();
            IPropertyProvider propertyProvider = serviceContainer.Single<IPropertyProvider>();

            IAbilityUpgradeService abilityUpgradeService = new AbilityUpgradeService(configurationProvider);
            CurrencyModel currencyModel = modelProvider.Get<CurrencyModel>();

            LevelUpModel levelUpModel = new(abilityUpgradeService, currencyModel);

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


            LevelUpSystemPresenter levelUpSystemPresenter = new(_levelUpSystemView, levelUpModel);

            LocalizationSystemPresenter localizationSystemPresenter = new(_localizationSystemView, localizationService);

            DeathPresenter deathPresenter = new(
                windowFsm,
                _deathView,
                gameStateMachine,
                gameLoopService,
                gamePauseService);

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

            _localizationSystemView.Construct(localizationSystemPresenter);
            _levelUpSystemView.Construct(levelUpSystemPresenter);
            _deathView.Construct(deathPresenter);
            _winView.Construct(winPresenter);
            _gameLoopView.Construct(gameLoopPresenter);
        }
    }
}