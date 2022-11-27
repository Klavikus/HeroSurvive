using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Views;
using CodeBase.MVVM.Views.HeroSelector;
using CodeBase.MVVM.Views.Upgrades;

namespace CodeBase.MVVM.Builders
{
    public class MainMenuViewBuilder
    {
        private readonly ViewFactory _viewFactory;

        public MainMenuViewBuilder(ViewFactory viewFactory) =>
            _viewFactory = viewFactory;

        public void Build()
        {
            UserNameSetterView userNameSetterView = _viewFactory.CreateUserNameView();
            StartMenuView startMenuView = _viewFactory.CreateStartMenu();
            HeroSelectorView heroSelector = _viewFactory.CreateHeroSelectorView();
            HeroView[] heroViews = _viewFactory.CreateHeroViews();
            PropertyView[] propertyViews = _viewFactory.CreatePropertyViews();
            UpgradesSelectorView upgradesSelectorView = _viewFactory.CreateUpgradesSelectorView();


            heroSelector.SetHeroViews(heroViews);
            heroSelector.SetPropertyViews(propertyViews);

            // startMenuView.Show();
            // heroSelector.Hide();
            // upgradesSelectorView.Hide();
        }
    }
}