using System;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.Services
{
    public class UpgradeService : IUpgradeService
    {
        private readonly IModelProvider _modelProvider;

        public event Action Updated;

        public UpgradeService(IModelProvider modelProvider)
        {
            _modelProvider = modelProvider ?? throw new ArgumentNullException(nameof(modelProvider));
        }

        private MainProperties Recalculate()
        {
            MainProperties resultProperties = new();

            foreach (UpgradeModel upgradeModel in _modelProvider.Get<UpgradeModel[]>())
                resultProperties += upgradeModel.Properties;

            return resultProperties;
        }

        public MainProperties GetUpgradesPropertiesData() =>
            Recalculate();

        public void AddProperties(UpgradeModel upgradeModel) =>
            Updated?.Invoke();
    }
}