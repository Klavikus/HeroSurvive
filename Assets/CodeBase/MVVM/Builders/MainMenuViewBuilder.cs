using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Views;

namespace CodeBase.MVVM.Builders
{
    public class MainMenuViewBuilder
    {
        private readonly ViewFactory _viewFactory;
        
        public MainMenuViewBuilder(ViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void Build()
        {
           HeroSelectorView heroSelector = _viewFactory.CreateHeroSelectorView();
           HeroView[] heroViews = _viewFactory.CreateHeroViews();
           PropertyView[] propertyViews = _viewFactory.CreatePropertyViews();
           HeroDescriptionView description = _viewFactory.CreateHeroDescriptionView();
           BaseAbilityView baseAbilityView = _viewFactory.CreateBaseAbilityView();
           
           
           heroSelector.SetHeroViews(heroViews);
           heroSelector.SetPropertyViews(propertyViews);
           heroSelector.SetDescriptionView(description);
           heroSelector.SetBaseAbilityView(baseAbilityView);
        }
    }
}