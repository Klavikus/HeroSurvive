using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Infrastructure.Core.Services.Providers;
using UnityEngine;
using ConfigurationContainer = GameCore.Source.Domain.Configs.ConfigurationContainer;

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
            ConfigurationContainer configurationContainer = new ResourceProvider().Load<ConfigurationContainer>();
            IConfigurationProvider configurationProvider = new ConfigurationProvider(configurationContainer);
            ICoroutineRunner coroutineRunner = new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();
         
           
            IResourceProvider resourceProvider = new ResourceProvider();
            _services.RegisterAsSingle(resourceProvider);

            
            _services.RegisterAsSingle<IGameStateMachine>(_gameStateMachine);
            _services.RegisterAsSingle(coroutineRunner);
            _services.RegisterAsSingle(configurationProvider);

            _services.LockRegister();
        }

        private void EnterLoadLevel() => 
            _gameStateMachine.Enter<MainMenuState>();
    }
}