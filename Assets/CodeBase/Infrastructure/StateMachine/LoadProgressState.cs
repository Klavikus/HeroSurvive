using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Presentation;

namespace CodeBase.Infrastructure
{
    public class LoadProgressState : IState
    {
        private const string MainMenuScene = GameConstants.MainMenuScene;

        private readonly GameStateMachine _gameStateMachine;

        private IPersistentDataService _persistentDataService;
        private IAdsProvider _adsProvider;
        private ITranslationService _translationService;
        private IPropertyProvider _propertyProvider;

        public LoadProgressState(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void Enter()
        {
            _persistentDataService = AllServices.Container.AsSingle<IPersistentDataService>();
            _translationService = AllServices.Container.AsSingle<ITranslationService>();
            _propertyProvider = AllServices.Container.AsSingle<IPropertyProvider>();
            _adsProvider = AllServices.Container.AsSingle<IAdsProvider>();
            IGameLoopService gameLoopService = AllServices.Container.AsSingle<IGameLoopService>();

            IBuilder modelBuilder = AllServices.Container.AsSingle<IModelBuilder>();
            IProvider modelProvider = AllServices.Container.AsSingle<IModelProvider>();

            IBuilder viewModelBuilder = AllServices.Container.AsSingle<IViewModelBuilder>();
            IProvider viewModelProvider = AllServices.Container.AsSingle<IViewModelProvider>();

            PrepareModels(modelBuilder, modelProvider);
            PrepareViewModels(viewModelBuilder, viewModelProvider);

            _persistentDataService.Initialize();
            _persistentDataService.LoadOrDefaultUpgradeModelsFromLocal();
            _propertyProvider.Initialize();

            _adsProvider.Initialized += AdsProviderOnInitialized;
            _adsProvider.Initialize();

            gameLoopService.Initialize();
        }

        public void Exit()
        {
            _adsProvider.Initialized -= AdsProviderOnInitialized;
        }

        private void AdsProviderOnInitialized()
        {
            _translationService.UpdateLanguage();
            _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);
        }

        private void PrepareModels(IBuilder modelBuilder, IProvider modelProvider)
        {
            modelProvider.Bind(modelBuilder.Build<HeroModel>());
            modelProvider.Bind(modelBuilder.Build<PropertiesModel>());
            modelProvider.Bind(modelBuilder.Build<MenuModel>());
            modelProvider.Bind(modelBuilder.Build<UpgradeModel[]>());
            modelProvider.Bind(modelBuilder.Build<CurrencyModel>());
            modelProvider.Bind(modelBuilder.Build<GameLoopModel>());
            modelProvider.Bind(modelBuilder.Build<LevelUpModel>());

            modelProvider.Get<LevelUpModel>().Bind(modelProvider.Get<CurrencyModel>());
        }

        private void PrepareViewModels(IBuilder viewModelBuilder, IProvider viewModelProvider)
        {
            viewModelProvider.Bind(viewModelBuilder.Build<HeroSelectorViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<MainPropertiesViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<MenuViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<UpgradeViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<CurrencyViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<LeaderBoardsViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<GameLoopViewModel>());
            viewModelProvider.Bind(viewModelBuilder.Build<LevelUpViewModel>());

            viewModelProvider.Get<LeaderBoardsViewModel>().Initialize();
            viewModelProvider.Get<GameLoopViewModel>().Bind(viewModelProvider.Get<LeaderBoardsViewModel>());
            viewModelProvider.Get<GameLoopViewModel>().Bind(viewModelProvider.Get<CurrencyViewModel>());
        }
    }
}