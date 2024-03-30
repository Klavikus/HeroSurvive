using Source.Application.Factories;
using Source.Infrastructure.Api.GameFsm;
using Source.Infrastructure.Core;
using Source.Infrastructure.Core.Services.DI;

namespace Source.Application.GameFSM.States
{
    public class MainMenuState : IState
    {
        private const string MainMenuScene = "MainMenu";

        private readonly SceneLoader _sceneLoader;
        private readonly ServiceContainer _serviceContainer;

        public MainMenuState(SceneLoader sceneLoader, ServiceContainer serviceContainer)
        {
            _sceneLoader = sceneLoader;
            _serviceContainer = serviceContainer;
        }

        public void Enter() => 
            _sceneLoader.Load(MainMenuScene, OnSceneLoaded);

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