namespace Source.Infrastructure.Api.GameFsm
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Update();
        void GoToGameLoop();
        void GoToMainMenu();
    }
}