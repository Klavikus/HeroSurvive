using CodeBase.Infrastructure.Builders;

namespace CodeBase.Infrastructure.Factories
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