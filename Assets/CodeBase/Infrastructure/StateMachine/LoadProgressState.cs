namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private const string MainMenuScene = "MainMenu";

        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine) => 
            _gameStateMachine = gameStateMachine;

        public void Enter() => 
            _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);

        public void Exit() { }
    }
}