using System.Collections;
using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;
using FMODUnity;

namespace CodeBase.Infrastructure
{
    public class BootstrapState : IState
    {
        private const string InitialScene = GameConstants.InitialSceneName;

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
            _coroutineRunner.StartCoroutine(PrepareFmod());
        }

        //TODO: Refactor this
        private IEnumerator PrepareFmod()
        {
            while (RuntimeManager.HaveAllBanksLoaded == false)
                yield return null;

            AllServices.Container.AsSingle<IAudioPlayerService>().Initialize();
            _sceneLoader.Load(InitialScene, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            IConfigurationProvider configurationProvider = new ConfigurationProvider(_configurationContainer);
            IViewModelProvider viewModelProvider = new ViewModelProvider();
            IModelProvider modelProvider = new ModelProvider();
            AbilityUpgradesProvider abilityUpgradesProvider = new AbilityUpgradesProvider(configurationProvider);

            ITranslationService translationService = new TranslationService();

            UpgradeDescriptionBuilder upgradeDescriptionBuilder =
                new UpgradeDescriptionBuilder(configurationProvider, translationService);
            LevelMapFactory levelMapFactory = new LevelMapFactory(configurationProvider);
            ModelFactory modelFactory = new ModelFactory(configurationProvider);
            EnemyFactory enemyFactory = new EnemyFactory(configurationProvider);
            IViewFactory viewFactory = new ViewFactory(
                configurationProvider,
                viewModelProvider,
                upgradeDescriptionBuilder);
            MainMenuViewBuilder mainMenuViewBuilder = new MainMenuViewBuilder(viewFactory);
            IMainMenuFactory mainMenuFactory = new MainMenuFactory(mainMenuViewBuilder);
            GameLoopViewFactory gameLoopViewFactory = new GameLoopViewFactory(
                configurationProvider,
                viewModelProvider,
                upgradeDescriptionBuilder);
            GameLoopViewBuilder gameLoopViewBuilder = new GameLoopViewBuilder(gameLoopViewFactory);

            IAudioPlayerService audioPlayerService = new AudioPlayerService(configurationProvider, _coroutineRunner);
            IAdsProvider adsProvider = new AdsProvider(_coroutineRunner);
            ISaveLoadService saveLoadService = new SaveLoadService(configurationProvider);
            IAuthorizeService authorizeService = new AuthorizeService();
            IAbilityUpgradeService abilityUpgradeService = new AbilityUpgradeService(configurationProvider);
            IUpgradeService upgradeService = new UpgradeService(modelProvider);
            IPersistentDataService persistentDataService =
                new PersistentDataService(configurationProvider, saveLoadService, modelProvider);
            ITargetService targetFinderService = new TargetFinderService(enemyFactory);
            IEnemySpawnService enemySpawnService = new EnemySpawnService(targetFinderService, enemyFactory);
            IVfxService vfxService = new VfxService(configurationProvider);
            ILeveCompetitionService leveCompetitionService =
                new LeveCompetitionService(enemySpawnService, configurationProvider, modelProvider, vfxService);
            PlayerEventHandler playerEventHandler = new PlayerEventHandler();

            IModelBuilder modelBuilder = new ModelBuilder(modelFactory, abilityUpgradeService);

            IViewModelBuilder viewModelBuilder =
                new ViewModelBuilder
                (
                    configurationProvider,
                    modelProvider,
                    translationService,
                    upgradeService,
                    authorizeService,
                    _coroutineRunner,
                    leveCompetitionService,
                    playerEventHandler,
                    adsProvider,
                    abilityUpgradeService,
                    audioPlayerService
                );

            IPropertyProvider propertyProvider = new PropertyProvider(
                configurationProvider,
                upgradeService,
                modelProvider);

            AbilityProjectionBuilder abilityProjectionBuilder =
                new AbilityProjectionBuilder(targetFinderService, audioPlayerService);
            AbilityFactory abilityFactory =
                new AbilityFactory(abilityProjectionBuilder, _coroutineRunner, abilityUpgradesProvider);
            PlayerBuilder playerBuilder = new PlayerBuilder(modelProvider, configurationProvider, propertyProvider,
                abilityUpgradeService, abilityFactory, audioPlayerService);
            AbilityBuilder abilityBuilder = new AbilityBuilder(playerBuilder);

            IGameLoopService gameLoopService = new GameLoopService(
                levelMapFactory,
                gameLoopViewBuilder,
                abilityBuilder,
                modelProvider,
                playerBuilder,
                leveCompetitionService,
                playerEventHandler,
                audioPlayerService);

            targetFinderService.BindPlayerBuilder(playerBuilder);
            abilityFactory.BindGameLoopService(gameLoopService);

            _services.RegisterAsSingle(viewModelBuilder);
            _services.RegisterAsSingle(viewModelProvider);
            _services.RegisterAsSingle(modelBuilder);
            _services.RegisterAsSingle(modelProvider);
            _services.RegisterAsSingle(enemySpawnService);
            _services.RegisterAsSingle(persistentDataService);
            _services.RegisterAsSingle(adsProvider);
            _services.RegisterAsSingle(translationService);
            _services.RegisterAsSingle(authorizeService);
            _services.RegisterAsSingle(configurationProvider);
            _services.RegisterAsSingle(upgradeService);
            _services.RegisterAsSingle(propertyProvider);
            _services.RegisterAsSingle(targetFinderService);
            _services.RegisterAsSingle(gameLoopService);
            _services.RegisterAsSingle(viewFactory);
            _services.RegisterAsSingle(mainMenuFactory);
            _services.RegisterAsSingle(audioPlayerService);
            _services.RegisterAsSingle(vfxService);
        }
    }
}