using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.Factories
{
    public class ViewFactory
    {
        private readonly HeroSelectorViewModel _heroSelectorViewModel;

        public ViewFactory(ConfigurationProvider selectorViewModel, HeroSelectorViewModel heroSelectorViewModel)
        {
            _heroSelectorViewModel = heroSelectorViewModel;
        }

        public HeroSelectorView CreateHeroSelectorView() => new HeroSelectorView();
    }
}