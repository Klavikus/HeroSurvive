using System.ComponentModel.Design;
using Source.Application.Factories;
using Source.Infrastructure.Api.GameFsm;
using Source.Infrastructure.Core;
using ServiceContainer = Source.Infrastructure.Core.Services.DI.ServiceContainer;

namespace Source.Application.GameFSM.States
{
    public class GameLoopState : IState
    {
        private const string GameLoopScene = "GameLoopScene";

        private readonly SceneLoader _sceneLoader;
        private readonly ServiceContainer _serviceContainer;

        public GameLoopState(SceneLoader sceneLoader, ServiceContainer serviceContainer)
        {
            _sceneLoader = sceneLoader;
            _serviceContainer = serviceContainer;
        }

        public void Enter() =>
            _sceneLoader.Load(GameLoopScene, OnSceneLoaded);

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void OnSceneLoaded() => 
            new SceneInitializer().Initialize(_serviceContainer);
    }
}