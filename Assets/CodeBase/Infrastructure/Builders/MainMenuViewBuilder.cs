using CodeBase.Presentation;

namespace CodeBase.Infrastructure
{
    public class MainMenuViewBuilder
    {
        private readonly IViewFactory _viewFactory;

        public MainMenuViewBuilder(IViewFactory viewFactory) =>
            _viewFactory = viewFactory;

        public void Build()
        {
            StartMenuView startMenuView = _viewFactory.CreateStartMenu();
            HeroSelectorView heroSelector = _viewFactory.CreateHeroSelectorView();
            HeroView[] heroViews = _viewFactory.CreateHeroViews();
            PropertyView[] propertyViews = _viewFactory.CreatePropertyViews();
            UpgradesSelectorView upgradesSelectorView = _viewFactory.CreateUpgradesSelectorView();

            SettingsView settingsView = _viewFactory.CreateSettingsView();
            
            heroSelector.SetHeroViews(heroViews);
            heroSelector.SetPropertyViews(propertyViews);
        }
    }
}