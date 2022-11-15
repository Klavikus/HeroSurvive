using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Domain;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.Services.UpgradeService
{
    class UpgradeService : IUpgradeService
    {
        private readonly UpgradeModel[] _upgradeModels;

        private MainProperties _resultProperties;

        public event Action Updated;

        public UpgradeService(UpgradeModel[] upgradeModels)
        {
            _upgradeModels = upgradeModels;

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
            UpgradeModel first = _upgradeModels.Where(model => model == upgradeModel).First();
            // int levelIndex = upgradeModel.CurrentLevel;
            // if (upgradeModel.CurrentLevel == upgradeModel.Data.Upgrades.Length)
            // {
            //     levelIndex--;
            // }
            //
            // foreach (AdditionalHeroProperty additional in upgradeModel.GetCurrentAdditionalProperties())
            //     _upgradeModels[upgradeModel].UpdateProperty(additional.BaseProperty, additional.Value);

            Updated?.Invoke();
        }
    }
}