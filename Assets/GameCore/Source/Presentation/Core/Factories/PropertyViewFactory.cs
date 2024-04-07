using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using GameCore.Source.Presentation.Core.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;
using Object = UnityEngine.Object;

namespace GameCore.Source.Presentation.Core.Factories
{
    public class PropertyViewFactory : IPropertyViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Func<IPropertyView, MainPropertyViewData, IPresenter> _presenterCreationStrategy;

        public PropertyViewFactory(
            IConfigurationProvider configurationProvider,
            Func<IPropertyView, MainPropertyViewData, IPresenter> presenterCreationStrategy)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _presenterCreationStrategy = presenterCreationStrategy ??
                                         throw new ArgumentNullException(nameof(presenterCreationStrategy));
        }

        public IPropertyView[] Create()
        {
            int count = _configurationProvider.BasePropertiesConfig.PropertiesData.Count;
            IPropertyView[] result = new IPropertyView[count];

            for (int i = 0; i < count; i++)
            {
                PropertyView view = Object
                    .Instantiate(_configurationProvider.SelectableHeroView)
                    .GetComponent<PropertyView>();

                IPresenter presenter =
                    _presenterCreationStrategy.Invoke(view,
                        _configurationProvider.BasePropertiesConfig.PropertiesData[i]);

                view.Construct(presenter);
                result[i] = view;
            }

            return result;
        }
    }
}