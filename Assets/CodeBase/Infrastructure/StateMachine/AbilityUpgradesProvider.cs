﻿using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Abilities;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.StateMachine
{
    public class AbilityUpgradesProvider
    {
        private IConfigurationProvider _configurationProvider;

        private List<AbilityData> _abilitiesData;
        private Dictionary<AbilityConfigSO, AbilityData> _configsByAbilityData;

        public IReadOnlyDictionary<AbilityConfigSO, AbilityData> ConfigsByAbilityData => _configsByAbilityData;

        public AbilityUpgradesProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _configsByAbilityData = new Dictionary<AbilityConfigSO, AbilityData>();
            InitializeUpgrades();
        }

        private void InitializeUpgrades()
        {
            foreach (AbilityConfigSO configSO in _configurationProvider.GetAbilityConfigs())
                _configsByAbilityData.Add(configSO, new AbilityData(configSO));
        }

        public List<AbilityData> GetUpgradesData() => _abilitiesData;
    }
}