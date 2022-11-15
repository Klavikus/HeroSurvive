using System;
using System.Collections.Generic;
using CodeBase.Domain.Abilities;
using CodeBase.Domain.Enemies;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;

public class LevelUpModel
{
    private const int ExperiencePerLevel = 10;

    private AbilityUpgradeData _selectedUpgrade;
    private int _currentExperience;
    private int _currentLevel;

    private List<AbilityData> _abilitiesData;
    private readonly CurrencyModel _currencyModel;
    private readonly IAbilityUpgradeService _abilityUpgradeService;

    public event Action<int> LevelChanged;
    public event Action<float> LevelProgressChanged;
    public event Action<AbilityUpgradeData> UpgradeSelected;

    //
    public event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;

    public LevelUpModel(List<AbilityData> abilitiesData, CurrencyModel currencyModel,
        IAbilityUpgradeService abilityUpgradeService)
    {
        _abilitiesData = abilitiesData;
        _currencyModel = currencyModel;
        _abilityUpgradeService = abilityUpgradeService;
        _currentLevel = 1;
    }

    public void SelectUpgrade(AbilityUpgradeData abilityUpgradeData)
    {
        _abilityUpgradeService.UseUpgrade(abilityUpgradeData);
        _selectedUpgrade = abilityUpgradeData;
        UpgradeSelected?.Invoke(_selectedUpgrade);
    }

    public void HandleRewardedKill(Enemy enemy)
    {
        _currentExperience += enemy.KillExperience;
        _currencyModel.Add(enemy.KillCurrency);

        //TODO: Handle multiple levelup upgrades selection
        while (_currentExperience >= ExperiencePerLevel * _currentLevel)
        {
            _currentExperience -= ExperiencePerLevel * _currentLevel;
            LevelUp();
        }

        LevelProgressChanged?.Invoke((float) _currentExperience / (ExperiencePerLevel * _currentLevel));
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

public interface IAbilityUpgradeService : IService
{
    public void UseUpgrade(AbilityUpgradeData abilityUpgradeData);
    public void ResetUpgrades();
}