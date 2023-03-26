using CodeBase.Configs;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
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
        }

        public void Enter(HeroData heroData)
        {
            _modelProvider.Get<GameLoopModel>().CloseLevelInvoked += OnLevelCloseInvoked;
            _sceneLoader.Load(GameConstants.GameLoopScene, OnLoaded);
        }

        public void Exit()
        {
            _gameLoopService.Stop();
            _modelProvider.Get<GameLoopModel>().CloseLevelInvoked -= OnLevelCloseInvoked;
        }

        private void OnLoaded()
        {
            _gameLoopService.Start();
        }

        private void OnLevelCloseInvoked() =>
            _gameStateMachine.Enter<LoadLevelState, string>(GameConstants.MainMenuScene);
    }
}