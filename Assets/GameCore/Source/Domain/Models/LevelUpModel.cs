using System;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;

namespace GameCore.Source.Domain.Models
{
    public class LevelUpModel
    {
        private const int ExperiencePerLevelSoft = 10;

        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly CurrencyModel _currencyModel;

        private AbilityUpgradeData _selectedUpgrade;
        private int _currentExperience;
        private int _currentLevel;

        public event Action<int> LevelChanged;
        public event Action<float> LevelProgressChanged;
        public event Action<AbilityUpgradeData> UpgradeSelected;

        public LevelUpModel(IAbilityUpgradeService abilityUpgradeService, CurrencyModel currencyModel)
        {
            _abilityUpgradeService =
                abilityUpgradeService ?? throw new ArgumentNullException(nameof(abilityUpgradeService));
            _currencyModel = currencyModel ?? throw new ArgumentNullException(nameof(currencyModel));
            _currentLevel = 1;
        }

        public int CurrentLevel => _currentLevel;

        public float CurrentCompletionProgress =>
            _currentExperience / ExperiencePerLevelSoft * _currentLevel * _currentLevel;

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

        public void HandleRewardedKill(RewardData enemyController)
        {
            _currentExperience += enemyController.KillExperience;
            _currencyModel.Add(enemyController.KillCurrency);

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