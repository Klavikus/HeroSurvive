using System;
using System.Linq;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
{
    class UpgradeService : IUpgradeService
    {
        private readonly IModelProvider _modelProvider;

        private MainProperties _resultProperties;

        public event Action Updated;

        public UpgradeService(IModelProvider modelProvider)
        {
            _modelProvider = modelProvider;
        }

        public void Initialize()
        {
            Recalculate();
        }

        private void Recalculate()
        {
            _resultProperties = new MainProperties();

            foreach (UpgradeModel upgradeModel in _modelProvider.Get<UpgradeModel[]>())
            {
                _resultProperties += upgradeModel.Properties;
            }
        }

        public MainProperties GetUpgradesPropertiesData()
        {
            Recalculate();
            return _resultProperties;
        }

        public void AddProperties(UpgradeModel upgradeModel)
        {
            UpgradeModel first = _modelProvider.Get<UpgradeModel[]>().First(model => model == upgradeModel);

            Updated?.Invoke();
        }
    }
}