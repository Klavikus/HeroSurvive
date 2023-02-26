using CodeBase.Configs;
using CodeBase.Domain.Additional;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ConfigurationContainer _configurationContainer;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AudioPlayer _audioPlayer;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            ConfigurationContainer configurationContainer, ICoroutineRunner coroutineRunner, AudioPlayer audioPlayer)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _configurationContainer = configurationContainer;
            _coroutineRunner = coroutineRunner;
            _audioPlayer = audioPlayer;
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
            ConfigurationProvider configurationProvider = new ConfigurationProvider(_configurationContainer);
            AudioPlayerService audioPlayerService = new AudioPlayerService(_audioPlayer);

            AdsProvider adsProvider = new AdsProvider(_coroutineRunner);

            TranslationService translationService = new TranslationService();
            AllServices.Container.RegisterAsSingle<ITranslationService>(translationService);

            SaveLoadService saveLoadService = new SaveLoadService(configurationProvider);

            ModelFactory modelFactory = new ModelFactory(configurationProvider);


            //Builders
            UpgradeDescriptionBuilder upgradeDescriptionBuilder =
                new UpgradeDescriptionBuilder(configurationProvider, translationService);
            LevelMapFactory levelMapFactory = new LevelMapFactory(configurationProvider);

            //Models
            HeroModel heroModel = new HeroModel();
            PropertiesModel propertiesModel = new PropertiesModel();
            MenuModel menuModel = new MenuModel();
            UpgradeModel[] upgradeModels = modelFactory.CreateUpgradeModels();
            CurrencyModel currencyModel = new CurrencyModel();
            GameLoopModel gameLoopModel = new GameLoopModel();

            ModelProvider modelProvider = new ModelProvider(
                gameLoopModel,
                upgradeModels,
                currencyModel);
            UpgradeService upgradeService = new UpgradeService(upgradeModels);

            PersistentDataService persistentDataService =
                new PersistentDataService(configurationProvider, saveLoadService, modelProvider);
            AllServices.Container.RegisterAsSingle<IPersistentDataService>(persistentDataService);
            AllServices.Container.RegisterAsSingle<IAdsProvider>(adsProvider);

            //ViewModels
            HeroSelectorViewModel heroSelectorViewModel =
                new HeroSelectorViewModel(heroModel, menuModel, gameLoopModel, translationService);
            MainPropertiesViewModel propertiesViewModel =
                new MainPropertiesViewModel(propertiesModel, translationService);
            MenuViewModel menuViewModel = new MenuViewModel(menuModel);
            UpgradeViewModel upgradeViewModel =
                new UpgradeViewModel(upgradeModels, currencyModel, upgradeService);
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

            _services.RegisterAsSingle<IConfigurationProvider>(configurationProvider);
            _services.RegisterAsSingle<IUpgradeService>(upgradeService);
            _services.RegisterAsSingle<IPropertyProvider>(propertyProvider);
            _services.RegisterAsSingle<IModelProvider>(modelProvider);

            AuthorizeService authorizeService = new AuthorizeService();
            _services.RegisterAsSingle<IAuthorizeService>(authorizeService);
            
            LeaderBoardsViewModel leaderBoardsViewModel = new LeaderBoardsViewModel(authorizeService, new[]
                    {new LeaderBoard(GameConstants.StageTotalKillsLeaderBoardKey)}, _coroutineRunner, menuModel);

            ViewModelProvider viewModelProvider = new ViewModelProvider(leaderBoardsViewModel, menuViewModel);
            _services.RegisterAsSingle<IViewModelProvider>(viewModelProvider);

            //GameLoopCompositionRoot
            AbilityUpgradesProvider abilityUpgradesProvider = new AbilityUpgradesProvider(configurationProvider);
            AbilityUpgradeService abilityUpgradeService = new AbilityUpgradeService(configurationProvider);
            EnemyFactory enemyFactory = new EnemyFactory(configurationProvider);
            TargetFinderService targetFinderService = new TargetFinderService(enemyFactory);
            AbilityProjectionBuilder abilityProjectionBuilder =
                new AbilityProjectionBuilder(targetFinderService, audioPlayerService);
            AbilityFactory abilityFactory =
                new AbilityFactory(abilityProjectionBuilder, _coroutineRunner, abilityUpgradesProvider);

            LevelUpModel levelUpModel = new LevelUpModel(currencyModel, abilityUpgradeService);

            PlayerBuilder playerBuilder = new PlayerBuilder(heroModel, configurationProvider, propertyProvider,
                levelUpModel, abilityUpgradeService, abilityFactory, audioPlayerService);
            targetFinderService.BindPlayerBuilder(playerBuilder);
            _services.RegisterAsSingle<ITargetService>(targetFinderService);

            AbilityBuilder abilityBuilder = new AbilityBuilder(playerBuilder);
            MainMenuFactory mainMenuFactory = new MainMenuFactory(mainMenuViewBuilder);

            EnemySpawnService enemySpawnService =
                new EnemySpawnService(targetFinderService, enemyFactory);
            _services.RegisterAsSingle<IEnemySpawnService>(enemySpawnService);

            LeveCompetitionService leveCompetitionService =
                new LeveCompetitionService(enemySpawnService, configurationProvider, levelUpModel);

            LevelUpViewModel levelUpViewModel = new LevelUpViewModel(levelUpModel, abilityUpgradeService, adsProvider);

            PlayerEventHandler playerEventHandler = new PlayerEventHandler();
            GameLoopViewModel gameLoopViewModel =
                new GameLoopViewModel(gameLoopModel, leveCompetitionService, playerEventHandler, currencyViewModel,
                    adsProvider, leaderBoardsViewModel);
            GameLoopViewFactory gameLoopViewFactory = new GameLoopViewFactory(configurationProvider, gameLoopViewModel,
                levelUpViewModel,
                upgradeDescriptionBuilder);
            GameLoopViewBuilder gameLoopViewBuilder = new GameLoopViewBuilder(gameLoopViewFactory);
            GameLoopService gameLoopService = new GameLoopService(levelMapFactory, gameLoopViewBuilder, abilityBuilder,
                heroModel, playerBuilder, gameLoopModel, leveCompetitionService, playerEventHandler);
            abilityFactory.BindGameLoopService(gameLoopService);

            _services.RegisterAsSingle<IGameLoopService>(gameLoopService);
            _services.RegisterAsSingle<IViewFactory>(viewFactory);
            _services.RegisterAsSingle<IMainMenuFactory>(mainMenuFactory);
        }
    }
}