using System;
using System.Linq;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.Services
{
    public class UpgradeService : IUpgradeService
    {
        private readonly UpgradeModel[] _upgradeModels;

        private MainProperties _resultProperties;

        public event Action Updated;

        public UpgradeService(UpgradeModel[] upgradeModels)
        {
            _upgradeModels = upgradeModels ?? throw new ArgumentNullException(nameof(upgradeModels));
            Recalculate();
        }

        private void Recalculate()
        {
            _resultProperties = new MainProperties();

            foreach (UpgradeModel upgradeModel in _upgradeModels)
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
            UpgradeModel first = _upgradeModels.First(model => model == upgradeModel);

            Updated?.Invoke();
        }
    }
}