using CodeBase.ForSort;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ConfigurationContainer _configurationContainer;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            ConfigurationContainer configurationContainer, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _configurationContainer = configurationContainer;
            _coroutineRunner = coroutineRunner;
            _services = AllServices.Container;
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialScene, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            Debug.Log("RegisterServices");
            ConfigurationProvider configurationProvider = new ConfigurationProvider(_configurationContainer);

            ModelFactory modelFactory = new ModelFactory(configurationProvider);

            //Builders
            UpgradeDescriptionBuilder upgradeDescriptionBuilder = new UpgradeDescriptionBuilder(configurationProvider);
            LevelMapFactory levelMapFactory = new LevelMapFactory(configurationProvider);

            //Models
            HeroModel heroModel = new HeroModel();
            PropertiesModel propertiesModel = new PropertiesModel();
            MenuModel menuModel = new MenuModel();
            UpgradeModel[] upgradeModels = modelFactory.CreateUpgradeModels();
            CurrencyModel currencyModel = new CurrencyModel();
            GameLoopModel gameLoopModel = new GameLoopModel();


            ModelProvider modelProvider = new ModelProvider(gameLoopModel);
            UpgradeService upgradeService = new UpgradeService(upgradeModels);

            //ViewModels
            HeroSelectorViewModel heroSelectorViewModel =
                new HeroSelectorViewModel(heroModel, menuModel, gameLoopModel);
            MainPropertiesViewModel propertiesViewModel = new MainPropertiesViewModel(propertiesModel);
            MenuViewModel menuViewModel = new MenuViewModel(menuModel);
            UpgradeViewModel upgradeViewModel = new UpgradeViewModel(upgradeModels, currencyModel, upgradeService);
            CurrencyViewModel currencyViewModel = new CurrencyViewModel(currencyModel);


            ViewFactory viewFactory = new ViewFactory(
                configurationProvider,
                heroSelectorViewModel,
                propertiesViewModel,
                menuViewModel,
                upgradeViewModel,
                currencyViewModel,
                upgradeDescriptionBuilder);


            MainMenuViewBuilder mainMenuViewBuilder = new MainMenuViewBuilder(viewFactory);

            PropertyProvider propertyProvider = new PropertyProvider(configurationProvider,
                upgradeService,
                heroModel,
                propertiesModel);

            propertyProvider.Initialize();


            _services.RegisterSingle<IConfigurationProvider>(configurationProvider);
            _services.RegisterSingle<IUpgradeService>(upgradeService);
            _services.RegisterSingle<IPropertyProvider>(propertyProvider);

            _services.RegisterSingle<IModelProvider>(modelProvider);

            //GameLoopCompositionRoot


            PlayerBuilder playerBuilder = new PlayerBuilder(heroModel, configurationProvider, propertyProvider);
            EnemyFactory enemyFactory = new EnemyFactory(configurationProvider);
            TargetFinderService targetFinderService = new TargetFinderService(playerBuilder, enemyFactory);
            _services.RegisterSingle<ITargetService>(targetFinderService);

            AbilityUpgradesProvider abilityUpgradesProvider = new AbilityUpgradesProvider(configurationProvider);

            AbilityProjectionBuilder abilityProjectionBuilder = new AbilityProjectionBuilder(targetFinderService);
            AbilityFactory abilityFactory =
                new AbilityFactory(abilityProjectionBuilder, _coroutineRunner, abilityUpgradesProvider);
            AbilityBuilder abilityBuilder = new AbilityBuilder(playerBuilder, abilityFactory);
            MainMenuFactory mainMenuFactory = new MainMenuFactory(mainMenuViewBuilder);


            EnemySpawnService enemySpawnService =
                new EnemySpawnService(targetFinderService, enemyFactory);
            _services.RegisterSingle<IEnemySpawnService>(enemySpawnService);


            LevelUpModel levelUpModel = new LevelUpModel(abilityUpgradesProvider.GetUpgradesData(), currencyModel);
            LeveCompetitionService leveCompetitionService =
                new LeveCompetitionService(enemySpawnService, configurationProvider, levelUpModel);

            LevelUpViewModel levelUpViewModel = new LevelUpViewModel(levelUpModel);

            PlayerEventHandler playerEventHandler = new PlayerEventHandler();
            GameLoopViewModel gameLoopViewModel =
                new GameLoopViewModel(gameLoopModel, leveCompetitionService, playerEventHandler, currencyViewModel);
            GameLoopViewFactory gameLoopViewFactory =
                new GameLoopViewFactory(configurationProvider, gameLoopViewModel, levelUpViewModel);
            GameLoopViewBuilder gameLoopViewBuilder = new GameLoopViewBuilder(gameLoopViewFactory);
            GameLoopService gameLoopService = new GameLoopService(levelMapFactory, gameLoopViewBuilder, abilityBuilder,
                heroModel, playerBuilder, gameLoopModel, leveCompetitionService, playerEventHandler);
            _services.RegisterSingle<IGameLoopService>(gameLoopService);


            _services.RegisterSingle<IMainMenuFactory>(mainMenuFactory);
        }
    }
}