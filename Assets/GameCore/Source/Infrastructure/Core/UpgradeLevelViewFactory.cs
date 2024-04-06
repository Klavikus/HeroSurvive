using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Presentation.Api.GameLoop;
using UnityEngine;

namespace GameCore.Source.Infrastructure.Core
{
    public class UpgradeLevelViewFactory : IViewFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public UpgradeLevelViewFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IUpgradeLevelView[] Create(int count)
        {
            IUpgradeLevelView[] result = new IUpgradeLevelView[count];

            for (int i = 0; i < count; i++)
                result[i] = Object
                    .Instantiate(_configurationProvider.UpgradeLevelView)
                    .GetComponent<IUpgradeLevelView>();

            return result;
        }
    }
}