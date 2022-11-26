using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Configs;
using CodeBase.Domain.Abilities;
using CodeBase.Domain.Enemies;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using UnityEngine;

public class LevelUpModel
{
    private const int ExperiencePerLevel = 10;

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
    event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;
    void UseUpgrade(AbilityUpgradeData abilityUpgradeData);
    void ResetUpgrades();
    void CalculateAvailableUpgrades();
    void BindToPlayer(Player player);
    AbilityUpgradeData[] GetAvailableUpgrades();
}

public class AbilityUpgradeService : IAbilityUpgradeService
{
    private readonly IConfigurationProvider _configurationProvider;

    private Dictionary<AbilityConfigSO, AbilityUpgradeViewData> _abilityConfigView =
        new Dictionary<AbilityConfigSO, AbilityUpgradeViewData>();

    private Dictionary<bool, AbilityUpgradeData[]> _availableUpgrades;

    private AbilityHandler _playerAbilityHandler;
    private Player _currentPlayer;

    public AbilityUpgradeService(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;

        _availableUpgrades = new Dictionary<bool, AbilityUpgradeData[]>();

        foreach (AbilityConfigSO abilityConfigSO in _configurationProvider.AbilityConfigs)
            _abilityConfigView.Add(abilityConfigSO, abilityConfigSO.UpgradeViewData);
    }

    public event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;

    public void UseUpgrade(AbilityUpgradeData abilityUpgradeData)
    {
        _playerAbilityHandler.UpdateAbility(abilityUpgradeData);

        Debug.Log("UseUpgrade");
    }


    public void ResetUpgrades()
    {
        foreach (Ability currentAbility in _playerAbilityHandler.CurrentAbilities)
            currentAbility.ResetUpgrades();

        Debug.Log("ResetUpgrades");
    }

    public void CalculateAvailableUpgrades() => Debug.Log("CalculateAvailableUpgrades");

    public AbilityUpgradeData[] GetAvailableUpgrades()
    {
        //Все доступные апгрейды, если есть пустой слот под апгрейд, если нет, то только апгрейды для текущих способностей, или ничего
        List<AbilityUpgradeData> resultData = new List<AbilityUpgradeData>();

        IReadOnlyList<Ability> _currentAbilities = GetPlayerAbilities();

        foreach (Ability currentAbility in _currentAbilities)
            if (currentAbility.CanUpgrade)
            {
                currentAbility.AvailableUpgrade.SetAbilityGainedStatus(true);
                resultData.Add(currentAbility.AvailableUpgrade);
            }

        if (CheckFreeSlot())
        {
            foreach (AbilityConfigSO abilityConfigSO in _configurationProvider.AbilityConfigs)
            {
                if (_currentAbilities.Select(ability => ability.CheckConfig(abilityConfigSO)).Contains(abilityConfigSO))
                    continue;

                abilityConfigSO.UpgradeData[0].SetAbilityGainedStatus(false);
                resultData.Add(abilityConfigSO.UpgradeData[0]);
            }
        }

        resultData = resultData.OrderBy(a => Guid.NewGuid()).ToList();

        return resultData.ToArray();
    }


    public void BindToPlayer(Player player)
    {
        _currentPlayer = player;
        _playerAbilityHandler = player.AbilityHandler;
        ResetAbilitiesGainedStatus();
    }

    private void ResetAbilitiesGainedStatus()
    {
        foreach (AbilityConfigSO abilityConfigSO in _configurationProvider.AbilityConfigs)
        foreach (AbilityUpgradeData abilityUpgradeData in abilityConfigSO.UpgradeData)
            abilityUpgradeData.SetAbilityGainedStatus(false);
    }

    private bool CheckFreeSlot() => _currentPlayer.IsFreeSlotAvailable;
    private IReadOnlyList<Ability> GetPlayerAbilities() => _playerAbilityHandler.CurrentAbilities;
}