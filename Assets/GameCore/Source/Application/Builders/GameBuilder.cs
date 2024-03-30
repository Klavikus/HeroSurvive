using GameCore.Source.Application.Factories;
using GameCore.Source.Application.GameFSM;
using GameCore.Source.Infrastructure.Core;

namespace GameCore.Source.Application.Builders
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