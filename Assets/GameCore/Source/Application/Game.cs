using Cysharp.Threading.Tasks;
using GameCore.Source.Application.GameFSM.States;
using GameCore.Source.Infrastructure.Api.GameFsm;

namespace GameCore.Source.Application
{
    internal class Game
    {
        private readonly IGameStateMachine _gameStateMachine;
    
        public Game(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
    
        public void Run() => 
            _gameStateMachine.Enter<BootstrapState>().Forget();

        public void Update() => 
            _gameStateMachine.Update();

        public void Finish()
        {
        }
    }
}