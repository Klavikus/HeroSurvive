using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.HeroSelectionService;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.MVVM.ViewModels;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ConfigurationContainer _configurationContainer;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            ConfigurationContainer configurationContainer)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _configurationContainer = configurationContainer;
            _services = AllServices.Container;
            RegisterServices();
        }

        public void Enter()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
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

            //ViewModels
            HeroSelectorViewModel heroSelectorViewModel = new HeroSelectorViewModel();

            ViewFactory viewFactory = new ViewFactory(configurationProvider, heroSelectorViewModel);

            MainMenuBuilder mainMenuBuilder = new MainMenuBuilder(viewFactory);
            
            HeroSelectionService heroSelectionService = new HeroSelectionService(configurationProvider);
            UpgradeService upgradeService = new UpgradeService();
            PropertyProvider propertyProvider = new PropertyProvider(configurationProvider, upgradeService, heroSelectionService);

            propertyProvider.Initialize();
            
            MainMenuFactory mainMenuFactory = new MainMenuFactory(_stateMachine, configurationProvider, propertyProvider);


            _services.RegisterSingle<IConfigurationProvider>(configurationProvider);
            _services.RegisterSingle<IHeroSelectionService>(heroSelectionService);
            _services.RegisterSingle<IUpgradeService>(upgradeService);
            _services.RegisterSingle<IPropertyProvider>(propertyProvider);
            _services.RegisterSingle<IMainMenuFactory>(mainMenuFactory);
        }
    }
}