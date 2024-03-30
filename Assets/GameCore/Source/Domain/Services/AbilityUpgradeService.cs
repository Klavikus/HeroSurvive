using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Domain.Services
{
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
            _playerAbilityHandler.UpgradeAbility(abilityUpgradeData);
        }

        public void ResetUpgrades()
        {
            foreach (AbilityController currentAbility in _playerAbilityHandler.CurrentAbilities)
                currentAbility.ResetUpgrades();
        }

        public void CalculateAvailableUpgrades()
        {
        }

        public AbilityUpgradeData[] GetAvailableUpgrades()
        {
            List<AbilityUpgradeData> resultData = new List<AbilityUpgradeData>();

            IReadOnlyList<AbilityController> _currentAbilities = GetPlayerAbilities();

            foreach (AbilityController currentAbility in _currentAbilities)
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
        private IReadOnlyList<AbilityController> GetPlayerAbilities() => _playerAbilityHandler.CurrentAbilities;
    }
}