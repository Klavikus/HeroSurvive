using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class AbilityHandler : MonoBehaviour, IAbilityHandler
    {
        private const int MaxAbilitySlots = 5;

        private readonly List<IAbilityController> _abilities = new();

        private readonly Dictionary<AbilityConfigSO, IAbilityController> _abilityByConfigSo = new();

        private bool _initialized;
        private int _currentAbilitySlots;
        private IReadOnlyDictionary<BaseProperty, float> _playerModifiers;
        private IAbilityFactory _abilityFactory;
        private IGameLoopService _gameLoopService;

        public bool IsFreeSlotAvailable => MaxAbilitySlots > _currentAbilitySlots;
        public IReadOnlyList<IAbilityController> CurrentAbilities => _abilities;

        public void Initialize(
            IAbilityFactory abilityFactory,
            IAudioPlayerService audioPlayerService,
            IGameLoopService gameLoopService)
        {
            _abilityFactory = abilityFactory;
            _gameLoopService = gameLoopService;
            _initialized = true;
        }

        public void AddAbility(AbilityConfigSO newAbilityConfigSO)
        {
            if (IsFreeSlotAvailable == false)
                throw new ArgumentException($"Add ability above limit is canceled! Limit is {MaxAbilitySlots}");

            IAbilityController newAbilityController =
                _abilityFactory.Create(newAbilityConfigSO, transform, _gameLoopService);

            _currentAbilitySlots++;
            _abilities.Add(newAbilityController);
            _abilityByConfigSo.Add(newAbilityConfigSO, newAbilityController);

            if (_playerModifiers != null)
                newAbilityController.UpdatePlayerModifiers(_playerModifiers);
        }

        public void UpgradeAbility(AbilityUpgradeData abilityUpgradeData)
        {
            if (_abilityByConfigSo.ContainsKey(abilityUpgradeData.BaseConfigSO))
                _abilityByConfigSo[abilityUpgradeData.BaseConfigSO].Upgrade();
            else
                AddAbility(abilityUpgradeData.BaseConfigSO);
        }

        public void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats)
        {
            _playerModifiers = stats;
            foreach (AbilityController ability in _abilities)
                ability.UpdatePlayerModifiers(stats);
        }

        private void LateUpdate()
        {
            if (_initialized == false)
                return;

            foreach (AbilityController ability in _abilities)
                ability.Execute();
        }

        private void OnDestroy()
        {
            foreach (AbilityController ability in _abilities)
                ability.Dispose();
        }
    }
}