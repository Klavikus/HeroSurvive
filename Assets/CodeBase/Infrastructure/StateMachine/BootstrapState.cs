using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.HeroSelectionService;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;
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

            //Models
            HeroModel heroModel = new HeroModel();
            PropertiesModel propertiesModel = new PropertiesModel();

            //ViewModels
            HeroSelectorViewModel heroSelectorViewModel = new HeroSelectorViewModel(heroModel);
            MainPropertiesViewModel propertiesViewModel = new MainPropertiesViewModel(propertiesModel);
            HeroDescriptionViewModel heroDescriptionViewModel = new HeroDescriptionViewModel(heroModel);
            BaseAbilityViewModel baseAbilityViewModel = new BaseAbilityViewModel(heroModel);
            
            ViewFactory viewFactory = new ViewFactory(configurationProvider, 
                heroSelectorViewModel,
                propertiesViewModel,
                heroDescriptionViewModel,
                baseAbilityViewModel);

            MainMenuViewBuilder mainMenuViewBuilder = new MainMenuViewBuilder(viewFactory);


            // HeroSelectionService heroSelectionService = new HeroSelectionService(heroSelectorViewModel);
            
            UpgradeService upgradeService = new UpgradeService();
            PropertyProvider propertyProvider = new PropertyProvider(configurationProvider,
                upgradeService,
                heroModel,
                propertiesModel);

            propertyProvider.Initialize();

            MainMenuFactory mainMenuFactory = new MainMenuFactory(mainMenuViewBuilder);


            _services.RegisterSingle<IConfigurationProvider>(configurationProvider);
            // _services.RegisterSingle<IHeroSelectionService>(heroSelectionService);
            _services.RegisterSingle<IUpgradeService>(upgradeService);
            _services.RegisterSingle<IPropertyProvider>(propertyProvider);
            _services.RegisterSingle<IMainMenuFactory>(mainMenuFactory);
        }
        
    }
}