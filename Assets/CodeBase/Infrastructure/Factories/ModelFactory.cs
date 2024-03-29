using CodeBase.Configs;
using CodeBase.Domain.Models;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.Factories
{
    public class ModelFactory : IModelFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ModelFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public UpgradeModel[] CreateUpgradeModels()
        {
            UpgradesConfigSO upgradesConfig = _configurationProvider.UpgradesConfig;
            UpgradeModel[] result = new UpgradeModel[upgradesConfig.UpgradeData.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new UpgradeModel(upgradesConfig.UpgradeData[i]);
            }

            return result;
        }
    }
}