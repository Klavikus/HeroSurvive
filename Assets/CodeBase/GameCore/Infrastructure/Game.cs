using CodeBase.GameCore.Infrastructure.Services;
using CodeBase.GameCore.Infrastructure.StateMachine;

namespace CodeBase.GameCore.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, ConfigurationContainer configurationContainer)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), configurationContainer,
                coroutineRunner);
        }
    }
}