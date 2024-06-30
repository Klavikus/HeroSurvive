using Cysharp.Threading.Tasks;

namespace GameCore.Source.Infrastructure.Api.GameFsm
{
    public interface IGameStateMachine
    {
        UniTask Enter<TState>() where TState : class, IState;
        void Update();
        void GoToGameLoop();
        void GoToMainMenu();
    }
}