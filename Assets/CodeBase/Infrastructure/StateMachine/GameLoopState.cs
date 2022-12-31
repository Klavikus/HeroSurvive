using CodeBase.Configs;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameLoopState : IPayloadedState<HeroData>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IModelProvider _modelProvider;
        private readonly IGameLoopService _gameLoopService;

        public GameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _modelProvider = AllServices.Container.AsSingle<IModelProvider>();
            _gameLoopService = AllServices.Container.AsSingle<IGameLoopService>();
            _modelProvider.GameLoopModel.CloseLevelInvoked += OnLevelCloseInvoked;
        }

        public void Enter(HeroData heroData)
        {
            _sceneLoader.Load(GameConstants.GameLoopScene, OnLoaded);
        }

        public void Exit()
        {
            _gameLoopService.Stop();
        }

        private void OnLoaded()
        {
            _gameLoopService.Start();
        }

        private void OnLevelCloseInvoked() =>
            _gameStateMachine.Enter<LoadLevelState, string>(GameConstants.MainMenuScene);
    }
}