using System.Linq;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Infrastructure.Core.Services.Providers;
using Modules.Common.Utils;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;

namespace GameCore.Source.Application.GameFSM.States
{
    public class BootstrapState : IState
    {
        private const string BootstrapScene = "Bootstrap";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceContainer _services;

        public BootstrapState
        (
            GameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            ServiceContainer serviceContainer
        )
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = serviceContainer;
        }

        public void Enter()
        {
            RegisterServices();

            _sceneLoader.Load(BootstrapScene, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void RegisterServices()
        {
            RegisterGameStateMachine();

            IConfigurationProvider configurationProvider = RegisterConfigurationProvider();
            ICoroutineRunner coroutineRunner = RegisterCoroutineRunner();
            IResourceProvider resourceProvider = RegisterResourceProvider();
            IGamePauseService gamePauseService = RegisterGamePauseService();
            ILocalizationService localizationService = RegisterLocalizationService(configurationProvider);

            IAudioPlayerService audioPlayerService = RegisterAudioPlayerService(
                configurationProvider,
                coroutineRunner,
                gamePauseService);

            IVfxService vfxService = RegisterVfxService(configurationProvider);
            IModelProvider modelProvider = RegisterModelProvider();

            PrepareModels(configurationProvider, modelProvider);

            localizationService.Initialize(new EnvironmentData(configurationProvider.BaseLanguage, false));

            _services.LockRegister();
        }

        private ILocalizationService RegisterLocalizationService(IConfigurationProvider configurationProvider)
        {
            ILocalizationService localizationService = new LocalizationService(configurationProvider);
            _services.RegisterAsSingle(localizationService);

            return localizationService;
        }

        private void RegisterGameStateMachine() =>
            _services.RegisterAsSingle<IGameStateMachine>(_gameStateMachine);

        private IConfigurationProvider RegisterConfigurationProvider()
        {
            ConfigurationContainer configurationContainer = new ResourceProvider().Load<ConfigurationContainer>();
            IConfigurationProvider configurationProvider = new ConfigurationProvider(configurationContainer);
            _services.RegisterAsSingle(configurationProvider);

            return configurationProvider;
        }

        private ICoroutineRunner RegisterCoroutineRunner()
        {
            ICoroutineRunner coroutineRunner = new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();
            _services.RegisterAsSingle(coroutineRunner);

            return coroutineRunner;
        }

        private IResourceProvider RegisterResourceProvider()
        {
            IResourceProvider resourceProvider = new ResourceProvider();
            _services.RegisterAsSingle(resourceProvider);

            return resourceProvider;
        }

        private IGamePauseService RegisterGamePauseService()
        {
            IMultiCallHandler multiCallHandler = new MultiCallHandler();
            IGamePauseService gamePauseService = new GamePauseService(multiCallHandler);
            _services.RegisterAsSingle(gamePauseService);

            return gamePauseService;
        }

        private IAudioPlayerService RegisterAudioPlayerService(IConfigurationProvider configurationProvider,
            ICoroutineRunner coroutineRunner, IGamePauseService gamePauseService)
        {
            IAudioPlayerService audioPlayerService = new AudioPlayerService(
                configurationProvider,
                coroutineRunner,
                gamePauseService);
            _services.RegisterAsSingle(audioPlayerService);

            return audioPlayerService;
        }

        private IVfxService RegisterVfxService(IConfigurationProvider configurationProvider)
        {
            IVfxService vfxService = new VfxService(configurationProvider);
            _services.RegisterAsSingle(vfxService);

            return vfxService;
        }

        private IModelProvider RegisterModelProvider()
        {
            IModelProvider modelProvider = new ModelProvider();
            _services.RegisterAsSingle(modelProvider);

            return modelProvider;
        }

        private void EnterLoadLevel() =>
            _gameStateMachine.Enter<MainMenuState>();

        private void PrepareModels(IConfigurationProvider configurationProvider, IModelProvider modelProvider)
        {
            //TODO: Move to LoadDataState
            UpgradesConfigSO upgradesConfig = configurationProvider.UpgradesConfig;
            UpgradeModel[] result = new UpgradeModel[upgradesConfig.UpgradeData.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = new UpgradeModel(upgradesConfig.UpgradeData[i]);
            modelProvider.Bind(result);

            PropertiesModel propertiesModel = new PropertiesModel();
            modelProvider.Bind(propertiesModel);

            HeroModel heroModel = new HeroModel();
            HeroData[] availableHeroesData =
                configurationProvider.HeroConfig.HeroesData.Select(heroData => heroData).ToArray();
            heroModel.SetHeroData(availableHeroesData.First());
            modelProvider.Bind(heroModel);

            CurrencyModel currencyModel = new CurrencyModel();
            modelProvider.Bind(currencyModel);
        }
    }
}