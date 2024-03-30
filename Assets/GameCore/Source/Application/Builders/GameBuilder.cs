using Source.Application.Factories;
using Source.Application.GameFSM;
using Source.Infrastructure.Core;

namespace Source.Application.Builders
{
    internal class GameBuilder
    {
        public Game Build()
        {
            SceneLoader sceneLoader = new SceneLoaderFactory().Create();

            return new Game(new GameStateMachine(sceneLoader));
        }
    }
}