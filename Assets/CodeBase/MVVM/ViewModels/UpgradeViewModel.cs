using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Domain.Data;
using CodeBase.Extensions;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class UpgradeViewModel
    {
        private readonly UpgradeModel[] _upgrades;
        private readonly CurrencyModel _currencyModel;
        private readonly Dictionary<UpgradeData, UpgradeModel> _upgradeModels;
        private readonly IUpgradeService _upgradeService;

        private UpgradeData _currentSelected;

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

            _currentSelected = _upgrades[0].Data;
            SelectUpgrade(_currentSelected);
        }

        public event Action<UpgradeData, int> Upgraded;
        public event Action<UpgradeData, int> UpgradeSelected;

        private int MaxUpgradeId => _upgrades.Length - 1;

        private int CurrentSelectedUpgradeId =>
            _upgrades.TakeWhile(upgrade => upgrade.Data != _currentSelected).Count();

        private void OnUpgradeChanged(UpgradeModel upgradeModel)
        {
            _upgradeService.AddProperties(upgradeModel);
            Upgraded?.Invoke(upgradeModel.Data, upgradeModel.CurrentLevel);
        }

        public void SelectUpgrade(UpgradeData upgradeData)
        {
            _currentSelected = upgradeData;
            UpgradeSelected?.Invoke(upgradeData, _upgradeModels[upgradeData].CurrentLevel);
        }

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

        public void HandleMove(int dX, int dY, int rowCount, int colCount)
        {
            if (dX != 0)
                HandleHorizontalScroll(dX);

            if (dY != 0)
                HandleVerticalScroll(dY, rowCount, colCount);
        }

        private void HandleVerticalScroll(int dY, int rowCount, int colCount)
        {
            int[] currentPosition = CurrentSelectedUpgradeId.ConvertIndexFromLinear(rowCount, colCount);

            int newLinearIndex = new[] {currentPosition[0] + dY, currentPosition[1]}.ConvertIndexToLinear(colCount);

            if (newLinearIndex.ContainsInInterval(0, MaxUpgradeId))
                SelectUpgrade(_upgrades[newLinearIndex].Data);
        }

        private void HandleHorizontalScroll(int dX)
        {
            int newLinearIndex = CurrentSelectedUpgradeId + dX;

            if (newLinearIndex.ContainsInInterval(0, MaxUpgradeId))
                SelectUpgrade(_upgrades[newLinearIndex].Data);
        }
    }
}