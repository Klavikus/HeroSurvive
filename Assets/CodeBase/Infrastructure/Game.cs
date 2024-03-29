using CodeBase.Domain;

namespace CodeBase.Infrastructure
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