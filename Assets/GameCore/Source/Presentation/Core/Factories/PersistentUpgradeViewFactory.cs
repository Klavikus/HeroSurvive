using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using GameCore.Source.Presentation.Api;
using GameCore.Source.Presentation.Api.GameLoop;
using GameCore.Source.Presentation.Core.MainMenu.Upgrades;
using Modules.MVPPassiveView.Runtime;
using Object = UnityEngine.Object;

namespace GameCore.Source.Presentation.Core.Factories
{
    public class PersistentUpgradeViewFactory : IPersistentUpgradeViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Func<UpgradeData, IPersistentUpgradeView, IPresenter> _presenterCreationStrategy;

        public PersistentUpgradeViewFactory(
            IConfigurationProvider configurationProvider,
            Func<UpgradeData, IPersistentUpgradeView, IPresenter> presenterCreationStrategy)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _presenterCreationStrategy = presenterCreationStrategy ??
                                         throw new ArgumentNullException(nameof(presenterCreationStrategy));
        }

        public IPersistentUpgradeView[] Create()
        {
            int count = _configurationProvider.UpgradesConfig.UpgradeData.Length;
            IPersistentUpgradeView[] result = new IPersistentUpgradeView[count];

            for (int i = 0; i < count; i++)
            {
                PersistentUpgradeView view = Object
                    .Instantiate(_configurationProvider.PersistentUpgradeView)
                    .GetComponent<PersistentUpgradeView>();

                IPresenter presenter =
                    _presenterCreationStrategy.Invoke(_configurationProvider.UpgradesConfig.UpgradeData[i], view);

                view.Construct(presenter);
                result[i] = view;
            }

            return result;
        }
    }
}