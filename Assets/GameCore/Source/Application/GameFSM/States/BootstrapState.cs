using System;
using System.ComponentModel.Composition;
using System.Linq;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Handlers;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Controllers.Core.Handlers;
using GameCore.Source.Controllers.Core.Providers;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Data.Dto;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core;
using GameCore.Source.Infrastructure.Core.Services;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Infrastructure.Core.Services.Providers;
using Modules.Common.Utils;
using Modules.DAL.Implementation.Data;
using Modules.DAL.Implementation.Data.Entities;
using Modules.DAL.Implementation.DataContexts;
using Modules.DAL.Implementation.Repositories;
using Modules.DAL.Implementation.Services;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public async void Enter()
        {
            await _sceneLoader.LoadAsync(BootstrapScene);

            RegisterServices();

            _gameStateMachine.Enter<LoadDataState>();
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
            IGameLoopService gameLoopService = RegisterGameLoopService();
            IUpgradeService upgradeService = RegisterUpgradeService(modelProvider);

            IPropertyProvider propertyProvider = RegisterPropertyProvider(
                configurationProvider,
                upgradeService,
                modelProvider);

            IUpgradeDescriptionBuilder descriptionBuilder =
                new UpgradeDescriptionBuilder(configurationProvider, localizationService);
            _services.RegisterAsSingle(descriptionBuilder);

            IAdsProvider adsProvider = new YandexGamesAdsProvider();
            _services.RegisterAsSingle(adsProvider);

            // PrepareModels(configurationProvider, modelProvider);

            localizationService.Initialize(new EnvironmentData(configurationProvider.BaseLanguage, false));

#if DAL_YANDEX_GAME_PLUGIN
            Type[] repoTypes =
            {
                typeof(SyncData),
                typeof(CurrencyDto),
                typeof(AccountDto),
                typeof(UpgradeDto),
                typeof(SettingsDto)
            };
            GameData data = new(repoTypes);

            JsonPrefsDataContext localDataContext = new(data, nameof(JsonPrefsDataContext));
            YandexPluginGameContext cloudDataContext = new(data);

            CompositeRepository localRepository = new(localDataContext, repoTypes);
            CompositeRepository cloudRepository = new(cloudDataContext, repoTypes);

            LocalCloudContextService contextService = new(localRepository, cloudRepository);
            IProgressService progressService = new ProgressService(localRepository, contextService);
            _services.RegisterAsSingle(progressService);
#endif

            ApplicationFocusChangeHandler focusChangeHandler =
                new GameObject("FocusChangeHandler").AddComponent<ApplicationFocusChangeHandler>();
            Object.DontDestroyOnLoad(focusChangeHandler);
            _services.RegisterAsSingle<IApplicationFocusChangeHandler>(focusChangeHandler);

            _services.LockRegister();
        }

        private IPropertyProvider RegisterPropertyProvider(IConfigurationProvider configurationProvider,
            IUpgradeService upgradeService, IModelProvider modelProvider)
        {
            IPropertyProvider propertyProvider = new PropertyProvider(
                configurationProvider,
                upgradeService,
                modelProvider);
            _services.RegisterAsSingle(propertyProvider);

            return propertyProvider;
        }

        private IUpgradeService RegisterUpgradeService(IModelProvider modelProvider)
        {
            IUpgradeService upgradeService = new UpgradeService(modelProvider);
            _services.RegisterAsSingle(upgradeService);

            return upgradeService;
        }

        private IGameLoopService RegisterGameLoopService()
        {
            IGameLoopService gameLoopService = new GameLoopService();
            _services.RegisterAsSingle(gameLoopService);

            return gameLoopService;
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

            gamePauseService.Paused += () => Time.timeScale = 0;
            gamePauseService.Resumed += () => Time.timeScale = 1;

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
    }
}