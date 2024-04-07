using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api.Factories;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using GameCore.Source.Presentation.Core.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;
using Object = UnityEngine.Object;

namespace GameCore.Source.Presentation.Core.Factories
{
    public class SelectableHeroViewFactory : ISelectableHeroViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Func<ISelectableHeroView, HeroData, IPresenter> _presenterCreationStrategy;

        public SelectableHeroViewFactory(
            IConfigurationProvider configurationProvider,
            Func<ISelectableHeroView, HeroData, IPresenter> presenterCreationStrategy)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _presenterCreationStrategy = presenterCreationStrategy ??
                                         throw new ArgumentNullException(nameof(presenterCreationStrategy));
        }

        public ISelectableHeroView[] Create()
        {
            int count = _configurationProvider.HeroConfig.HeroesData.Length;
            ISelectableHeroView[] result = new ISelectableHeroView[count];

            for (int i = 0; i < count; i++)
            {
                SelectableHeroView view = Object
                    .Instantiate(_configurationProvider.HeroConfig.BaseHeroView)
                    .GetComponent<SelectableHeroView>();

                IPresenter presenter =
                    _presenterCreationStrategy.Invoke(view, _configurationProvider.HeroConfig.HeroesData[i]);

                view.Construct(presenter);
                result[i] = view;
            }

            return result;
        }
    }
}