﻿using System;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Domain.Abilities
{
    public class AbilityHandler : MonoBehaviour, IAbilityHandler
    {
        private const int MaxAbilitySlots = 5;

        private readonly List<Ability> _abilities = new List<Ability>();
        private readonly Dictionary<AbilityConfigSO, Ability> _abilityByConfigSo = new Dictionary<AbilityConfigSO, Ability>();

        private bool _initialized;
        private int _currentAbilitySlots;
        private IReadOnlyDictionary<BaseProperty, float> _playerModifiers;
        private AbilityFactory _abilityFactory;

        public bool IsFreeSlotAvailable => MaxAbilitySlots > _currentAbilitySlots;
        public IReadOnlyList<Ability> CurrentAbilities => _abilities;

        public void Initialize(AbilityFactory abilityFactory, AudioPlayerService audioPlayerService)
        {
            _abilityFactory = abilityFactory;
            _initialized = true;
        }

        public void AddAbility(AbilityConfigSO newAbilityConfigSO)
        {
            if (IsFreeSlotAvailable == false)
                throw new ArgumentException($"Add ability above limit is canceled! Limit is {MaxAbilitySlots}");

            Ability newAbility = _abilityFactory.Create(newAbilityConfigSO);

            _currentAbilitySlots++;
            _abilities.Add(newAbility);
            _abilityByConfigSo.Add(newAbilityConfigSO, newAbility);
            newAbility.Initialize(transform);

            if (_playerModifiers != null)
                newAbility.UpdatePlayerModifiers(_playerModifiers);
        }

        public void UpdateAbility(AbilityUpgradeData abilityUpgradeData)
        {
            if (_abilityByConfigSo.ContainsKey(abilityUpgradeData.BaseConfigSO))
                _abilityByConfigSo[abilityUpgradeData.BaseConfigSO].Upgrade();
            else
                AddAbility(abilityUpgradeData.BaseConfigSO);
        }

        public void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats)
        {
            _playerModifiers = stats;
            foreach (Ability ability in _abilities)
                ability.UpdatePlayerModifiers(stats);
        }

        private void LateUpdate()
        {
            if (_initialized == false)
                return;

            foreach (Ability ability in _abilities)
                ability.Execute();
        }
    }
}