using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Domain.Services
{
    public class AbilityUpgradeService : IAbilityUpgradeService
    {
        private readonly IConfigurationProvider _configurationProvider;

        private Dictionary<AbilityConfigSO, AbilityUpgradeViewData> _abilityConfigView = new();

        private Dictionary<bool, AbilityUpgradeData[]> _availableUpgrades;

        private IAbilityHandler _abilityContainer;
        private PlayerModel _currentPlayer;

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
            _abilityContainer.UpgradeAbility(abilityUpgradeData);
        }

        public void ResetUpgrades()
        {
            foreach (IAbilityController currentAbility in _abilityContainer.CurrentAbilities)
                currentAbility.ResetUpgrades();
        }

        public void CalculateAvailableUpgrades()
        {
        }

        public AbilityUpgradeData[] GetAvailableUpgrades()
        {
            List<AbilityUpgradeData> resultData = new List<AbilityUpgradeData>();

            IReadOnlyList<IAbilityController> _currentAbilities = GetPlayerAbilities();

            foreach (IAbilityController currentAbility in _currentAbilities)
                if (currentAbility.CanUpgrade)
                {
                    currentAbility.AvailableUpgrade.SetAbilityGainedStatus(true);
                    resultData.Add(currentAbility.AvailableUpgrade);
                }

            if (CheckFreeSlot())
            {
                foreach (AbilityConfigSO abilityConfigSO in _configurationProvider.AbilityConfigs)
                {
                    if (_currentAbilities.Select(ability => ability.CheckConfig(abilityConfigSO))
                        .Contains(abilityConfigSO))
                        continue;

                    abilityConfigSO.UpgradeData[0].SetAbilityGainedStatus(false);
                    resultData.Add(abilityConfigSO.UpgradeData[0]);
                }
            }

            resultData = resultData.OrderBy(a => Guid.NewGuid()).ToList();

            return resultData.ToArray();
        }

        public void BindToPlayer(PlayerModel player)
        {
            _currentPlayer = player;
            _abilityContainer = player.AbilityContainer;
            ResetAbilitiesGainedStatus();
        }

        private void ResetAbilitiesGainedStatus()
        {
            foreach (AbilityConfigSO abilityConfigSO in _configurationProvider.AbilityConfigs)
            foreach (AbilityUpgradeData abilityUpgradeData in abilityConfigSO.UpgradeData)
                abilityUpgradeData.SetAbilityGainedStatus(false);
        }

        private bool CheckFreeSlot() => _currentPlayer.IsFreeSlotAvailable;
        private IReadOnlyList<IAbilityController> GetPlayerAbilities() =>
            _abilityContainer.CurrentAbilities;
    }
}