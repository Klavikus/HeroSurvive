using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Infrastructure.Factories;
using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IMainMenuFactory _mainMenuFactory;
        private readonly IModelProvider _modelProvider;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuFactory = AllServices.Container.AsSingle<IMainMenuFactory>();
            _modelProvider = AllServices.Container.AsSingle<IModelProvider>();
        }

        public void Enter(string sceneName)
        {
            _modelProvider.Get<GameLoopModel>().StartLevelInvoked += OnLevelInvoked;
            _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
        }

        public void Exit()
        {
            _modelProvider.Get<GameLoopModel>().StartLevelInvoked -= OnLevelInvoked;
            AllServices.Container.AsSingle<IAudioPlayerService>().StopMainMenuAmbient();
        }

        private void OnLoaded()
        {
            _mainMenuFactory.Initialization();
            AllServices.Container.AsSingle<IAudioPlayerService>().StartMainMenuAmbient();
        }

        private void OnLevelInvoked(HeroData heroData) => _gameStateMachine.Enter<GameLoopState, HeroData>(heroData);
    }
}