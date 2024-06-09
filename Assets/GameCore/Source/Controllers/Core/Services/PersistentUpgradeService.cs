using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Data.Dto;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.Services;

namespace GameCore.Source.Controllers.Core.Services
{
    public class PersistentUpgradeService : IPersistentUpgradeService
    {
        private readonly UpgradeModel[] _upgrades;
        private readonly CurrencyModel _currencyModel;
        private readonly Dictionary<UpgradeData, UpgradeModel> _upgradeModels;
        private readonly IUpgradeService _upgradeService;
        private readonly IAudioPlayerService _sfxService;
        private readonly IProgressService _progressService;

        private UpgradeData _currentSelected;

        public PersistentUpgradeService(
            UpgradeModel[] upgrades,
            CurrencyModel currencyModel,
            IUpgradeService upgradeService,
            IAudioPlayerService sfxService,
            IProgressService progressService)
        {
            _upgrades = upgrades;
            _currencyModel = currencyModel;
            _upgradeService = upgradeService;
            _sfxService = sfxService;
            _progressService = progressService;

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

        private async void OnUpgradeChanged(UpgradeModel upgradeModel)
        {
            _progressService.UpdateUpgradeData(upgradeModel.Data.KeyName, upgradeModel.CurrentLevel);
            await _progressService.Save();
            await _progressService.SyncWithCloud();
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
            _sfxService.PlayUpgradeBuy();
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

        public UpgradeData GetCurrentUpgradeData() =>
            _currentSelected;

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