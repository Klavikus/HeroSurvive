using CodeBase.GameCore.Infrastructure.Builders;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public class MainMenuFactory : IMainMenuFactory
    {
        private readonly MainMenuViewBuilder _mainMenuViewBuilder;

        public MainMenuFactory(MainMenuViewBuilder mainMenuViewBuilder)
        {
            _mainMenuViewBuilder = mainMenuViewBuilder;
        }

        public void Initialization()
        {
            _mainMenuViewBuilder.Build();
        }
    }
}