using Cysharp.Threading.Tasks;
using GameCore.Source.Application.Factories;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core.Services;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Core;

namespace GameCore.Source.Application.GameFSM.States
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

        public async UniTask Enter()
        {
            await _serviceContainer.Single<LoadingCurtain>().ShowAsync();
            await _sceneLoader.LoadAsync(MainMenuScene);
            new SceneInitializer().Initialize(_serviceContainer);
            await _serviceContainer.Single<LoadingCurtain>().HideAsync();
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}