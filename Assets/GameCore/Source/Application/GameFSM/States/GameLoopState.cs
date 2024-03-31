using GameCore.Source.Application.Factories;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core;
using ServiceContainer = GameCore.Source.Infrastructure.Core.Services.DI.ServiceContainer;

namespace GameCore.Source.Application.GameFSM.States
{
    public class GameLoopState : IState
    {
        private const string GameLoopScene = "GameLoop";

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