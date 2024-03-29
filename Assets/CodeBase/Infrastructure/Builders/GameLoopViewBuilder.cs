using CodeBase.Infrastructure.Factories;

namespace CodeBase.Infrastructure.Builders
{
    public class GameLoopViewBuilder
    {
        private readonly GameLoopViewFactory _gameLoopViewFactory;

        public GameLoopViewBuilder(GameLoopViewFactory gameLoopViewFactory) =>
            _gameLoopViewFactory = gameLoopViewFactory;

        public void Build() => _gameLoopViewFactory.CreateGameLoopView();
    }
}