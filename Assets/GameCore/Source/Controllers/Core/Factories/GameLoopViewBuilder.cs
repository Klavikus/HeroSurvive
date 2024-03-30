
namespace GameCore.Source.Controllers.Core.Factories
{
    public class GameLoopViewBuilder
    {
        private readonly GameLoopViewFactory _gameLoopViewFactory;

        public GameLoopViewBuilder(GameLoopViewFactory gameLoopViewFactory) =>
            _gameLoopViewFactory = gameLoopViewFactory;

        public void Build() => _gameLoopViewFactory.CreateGameLoopView();
    }
}