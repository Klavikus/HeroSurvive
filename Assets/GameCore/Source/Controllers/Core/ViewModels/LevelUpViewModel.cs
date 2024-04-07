using System;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;

namespace GameCore.Source.Controllers.Core.ViewModels
{
    public class LevelUpViewModel
    {
        private readonly LevelUpModel _levelUpModel;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly IAdsProvider _adsProvider;

        public event Action Rerolled;
        public event Action<int> LevelChanged;
        public event Action InvokedViewHide;
        public event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;
        public event Action<float> LevelProgressChanged;

        public LevelUpViewModel(
            LevelUpModel levelUpModel,
            IAbilityUpgradeService abilityUpgradeService,
            IAdsProvider adsProvider)
        {
            _levelUpModel = levelUpModel;
            _abilityUpgradeService = abilityUpgradeService;
            _adsProvider = adsProvider;
            _levelUpModel.LevelChanged += OnLevelChanged;
            _abilityUpgradeService.AvailableUpgradesChanged += OnAvailableUpgradesChanged;
            _levelUpModel.LevelProgressChanged += OnLevelProgressChanged;
            _levelUpModel.UpgradeSelected += OnUpgradeSelected;
        }

        public void SelectUpgrade(AbilityUpgradeData abilityUpgradeData) =>
            _levelUpModel.SelectUpgrade(abilityUpgradeData);

        public void ResetLevels() =>
            _levelUpModel.ResetLevels();

        public AbilityUpgradeData[] GetAvailableUpgrades() =>
            _abilityUpgradeService.GetAvailableUpgrades();

        public void Reroll() =>
            _adsProvider.ShowAds(onRewardCallback: () => Rerolled?.Invoke());

        private void OnUpgradeSelected(AbilityUpgradeData _) =>
            InvokedViewHide?.Invoke();

        private void OnAvailableUpgradesChanged(AbilityUpgradeData[] availableUpgrades) =>
            AvailableUpgradesChanged?.Invoke(availableUpgrades);

        private void OnLevelChanged(int currentLevel)
        {
            _abilityUpgradeService.CalculateAvailableUpgrades();
            LevelChanged?.Invoke(currentLevel);
        }

        private void OnLevelProgressChanged(float currentPercent) =>
            LevelProgressChanged?.Invoke(currentPercent);
    }
}