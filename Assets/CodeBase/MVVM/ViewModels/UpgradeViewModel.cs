using System;
using System.Collections.Generic;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class UpgradeViewModel
    {
        private UpgradeModel[] _upgrades;
        private readonly CurrencyModel _currencyModel;
        private Dictionary<UpgradeData, UpgradeModel> _upgradeModels;
        private readonly IUpgradeService _upgradeService;

        public UpgradeViewModel(
            UpgradeModel[] upgrades,
            CurrencyModel currencyModel,
            IUpgradeService upgradeService)
        {
            _upgrades = upgrades;
            _currencyModel = currencyModel;
            _upgradeService = upgradeService;

            _upgradeModels = new Dictionary<UpgradeData, UpgradeModel>();

            foreach (UpgradeModel upgradeModel in _upgrades)
                _upgradeModels.Add(upgradeModel.Data, upgradeModel);

            foreach (UpgradeModel upgradeModel in _upgrades)
                upgradeModel.LevelChanged += OnUpgradeChanged;
        }


        public event Action<UpgradeData, int> Upgraded;
        public event Action<UpgradeData, int> UpgradeSelected;

        private void OnUpgradeChanged(UpgradeModel upgradeModel)
        {
            _upgradeService.AddProperties(upgradeModel);
            Upgraded?.Invoke(upgradeModel.Data, upgradeModel.CurrentLevel);
        }

        public void SelectUpgrade(UpgradeData upgradeData) =>
            UpgradeSelected?.Invoke(upgradeData, _upgradeModels[upgradeData].CurrentLevel);

        public void Upgrade(UpgradeData upgradeData)
        {
            _currencyModel.Pay(_upgradeModels[upgradeData].GetUpgradeCost());
            _upgradeModels[upgradeData].LevelUp();
        }

        public void Reset(UpgradeData upgradeData)
        {
            _currencyModel.Add(_upgradeModels[upgradeData].GetResetCost());
            _upgradeModels[upgradeData].ResetLevel();
        }

        public bool CheckPay(UpgradeData upgradeData, int levelToCheck) =>
            _currencyModel.CheckPayAvailability(upgradeData.Upgrades[levelToCheck].Price);

        public int GetCurrentUpgradeLevel(UpgradeData upgradeData) => _upgradeModels[upgradeData].CurrentLevel;
    }
}