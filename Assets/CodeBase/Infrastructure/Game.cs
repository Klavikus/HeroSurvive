using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, ConfigurationContainer configurationContainer)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), configurationContainer);
        }
    }
}