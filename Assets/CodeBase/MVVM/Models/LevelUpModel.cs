using System;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;

namespace CodeBase.MVVM.Models
{
    public class LevelUpModel
    {
        private const int ExperiencePerLevelSoft = 10;

        private AbilityUpgradeData _selectedUpgrade;
        private int _currentExperience;
        private int _currentLevel;

        private readonly CurrencyModel _currencyModel;
        private readonly IAbilityUpgradeService _abilityUpgradeService;

        public event Action<int> LevelChanged;
        public event Action<float> LevelProgressChanged;
        public event Action<AbilityUpgradeData> UpgradeSelected;


        public LevelUpModel(CurrencyModel currencyModel,
            IAbilityUpgradeService abilityUpgradeService)
        {
            _currencyModel = currencyModel;
            _abilityUpgradeService = abilityUpgradeService;
            _currentLevel = 1;
        }

        public void SelectUpgrade(AbilityUpgradeData abilityUpgradeData)
        {
            if (abilityUpgradeData == null)
            {
                UpgradeSelected?.Invoke(_selectedUpgrade);
                return;
            }

            _abilityUpgradeService.UseUpgrade(abilityUpgradeData);
            _selectedUpgrade = abilityUpgradeData;
            UpgradeSelected?.Invoke(_selectedUpgrade);
        }

        public void HandleRewardedKill(Enemy enemy)
        {
            _currentExperience += enemy.KillExperience;
            _currencyModel.Add(enemy.KillCurrency);

            float currentLevelNeedExperience = ExperiencePerLevelSoft * _currentLevel * _currentLevel;
            //TODO: Handle multiple levelup upgrades selection
            while (_currentExperience >= currentLevelNeedExperience)
            {
                _currentExperience -= (int) currentLevelNeedExperience;
                LevelUp();
            }

            LevelProgressChanged?.Invoke(_currentExperience / currentLevelNeedExperience);
        }

        public void ResetLevels()
        {
            _currentLevel = 1;
            _currentExperience = 0;
            _abilityUpgradeService.ResetUpgrades();
            LevelChanged?.Invoke(_currentLevel);
            LevelProgressChanged?.Invoke(0);
        }

        private void LevelUp()
        {
            _currentLevel++;
            LevelChanged?.Invoke(_currentLevel);
        }
    }
}