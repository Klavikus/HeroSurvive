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
using GameCore.Source.Presentation.Core.MainMenu;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.Common.WindowFsm.Runtime.Implementation;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;
using IEnemySpawnService = GameCore.Source.Controllers.Api.Services.IEnemySpawnService;

namespace GameCore.Source.Application.CompositionRoots
{
    public class GameLoopCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private GameLoopView _gameLoopView;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(GameLoopWindow)] = new MainMenuWindow(),
            };

            WindowFsm<GameLoopWindow> windowFsm = new WindowFsm<GameLoopWindow>(windows);

            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            IConfigurationProvider configurationProvider = serviceContainer.Single<IConfigurationProvider>();
            IAudioPlayerService audioPlayerService = serviceContainer.Single<IAudioPlayerService>();
            IVfxService vfxService = serviceContainer.Single<IVfxService>();
            ICoroutineRunner coroutineRunner = serviceContainer.Single<ICoroutineRunner>();
            IGamePauseService gamePauseService = serviceContainer.Single<IGamePauseService>();

            IModelProvider modelProvider = serviceContainer.Single<IModelProvider>();

            UpgradeModel[] upgradeModels = modelProvider.Get<UpgradeModel[]>();
            HeroModel heroModel = modelProvider.Get<HeroModel>();
            GameLoopModel gameLoopModel = new GameLoopModel();
            PropertiesModel propertiesModel = new PropertiesModel();
            PlayerModel playerModel = new PlayerModel();

            modelProvider.Bind(propertiesModel);

            IUpgradeService upgradeService = new UpgradeService(upgradeModels);

            IPropertyProvider propertyProvider = new PropertyProvider(
                configurationProvider,
                upgradeService,
                modelProvider);

            IAbilityUpgradeService abilityUpgradeService = new AbilityUpgradeService(configurationProvider);

            EnemyFactory enemyFactory = new EnemyFactory(configurationProvider, vfxService, audioPlayerService);

            ITargetService targetService = new TargetService(enemyFactory, playerModel);
            AbilityProjectionBuilder abilityProjectionBuilder = new AbilityProjectionBuilder(
                targetService,
                audioPlayerService,
                coroutineRunner);
            AbilityFactory abilityFactory = new AbilityFactory(abilityProjectionBuilder, coroutineRunner);

            PlayerBuilder playerBuilder = new PlayerBuilder(
                heroModel,
                configurationProvider,
                propertyProvider,
                abilityUpgradeService,
                abilityFactory,
                audioPlayerService,
                playerModel);

            AbilityBuilder abilityBuilder = new AbilityBuilder(playerBuilder);

            IEnemySpawnService enemySpawnService = new EnemySpawnService(targetService, enemyFactory);

            ILeveCompetitionService levelCompetitionService = new LeveCompetitionService(
                enemySpawnService,
                configurationProvider,
                modelProvider,
                vfxService);

            PlayerEventHandler playerEventHandler = new PlayerEventHandler();

            GameLoopService gameLoopService = new GameLoopService(
                abilityBuilder,
                playerBuilder,
                levelCompetitionService,
                playerEventHandler,
                audioPlayerService,
                gamePauseService,
                gameLoopModel,
                heroModel,
                targetService);

            gameLoopService.Initialize();
            propertyProvider.Initialize();

            GameLoopPresenter gameLoopPresenter = new(
                windowFsm,
                _gameLoopView,
                gameStateMachine,
                gameLoopService);

            _gameLoopView.Construct(gameLoopPresenter);
        }
    }
}