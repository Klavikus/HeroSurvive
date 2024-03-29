using System;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;

namespace CodeBase.Domain.Models
{
    public class LevelUpModel
    {
        private const int ExperiencePerLevelSoft = 10;

        private readonly IAbilityUpgradeService _abilityUpgradeService;

        private AbilityUpgradeData _selectedUpgrade;
        private CurrencyModel _currencyModel;
        private int _currentExperience;
        private int _currentLevel;

        public event Action<int> LevelChanged;
        public event Action<float> LevelProgressChanged;
        public event Action<AbilityUpgradeData> UpgradeSelected;


        public LevelUpModel(IAbilityUpgradeService abilityUpgradeService)
        {
            _abilityUpgradeService = abilityUpgradeService;
            _currentLevel = 1;
        }

        public void Bind(CurrencyModel currencyModel) => _currencyModel = currencyModel;

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