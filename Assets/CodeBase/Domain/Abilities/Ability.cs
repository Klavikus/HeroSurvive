﻿using System.Collections;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Additional;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Pools;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Domain.Abilities
{
    public class Ability
    {
        private readonly AbilityData _abilityData;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ProjectionPool _projectionPool;
        private readonly AbilityProjectionBuilder _projectionBuilder;
        private readonly AbilityUpgradeData[] _abilityUpgradesData;
        private readonly IGameLoopService _gameLoopService;

        private Transform _pivotObject;
        private bool _onCooldown;
        private bool _isInitialized;
        private int _currentUpgradeLevel;
        private Coroutine _activateProjectionCoroutine;

        public Ability(
            AbilityProjectionBuilder projectionBuilder,
            AbilityData abilityData,
            ICoroutineRunner coroutineRunner,
            ProjectionPool projectionPool,
            AbilityUpgradeData[] abilityUpgradesData,
            IGameLoopService gameLoopService)
        {
            _projectionBuilder = projectionBuilder;
            _abilityData = abilityData;
            _coroutineRunner = coroutineRunner;
            _projectionPool = projectionPool;
            _abilityUpgradesData = abilityUpgradesData;
            _gameLoopService = gameLoopService;
            _gameLoopService.LevelCloseInvoked += OnGameCloseInvoked;
        }

        ~Ability() => _gameLoopService.LevelCloseInvoked -= OnGameCloseInvoked;

        private void OnGameCloseInvoked()
        {
            _coroutineRunner.Stop(_activateProjectionCoroutine);
            _projectionPool.Clear();
        }

        public bool CanUpgrade => _currentUpgradeLevel < _abilityUpgradesData.Length;
        public AbilityUpgradeData AvailableUpgrade => _abilityUpgradesData[_currentUpgradeLevel];

        public void Initialize(Transform pivotObject)
        {
            _pivotObject = pivotObject;
            _isInitialized = true;
        }

        public void Execute()
        {
            if (_onCooldown || !_isInitialized)
                return;

            _activateProjectionCoroutine = _coroutineRunner.Run(ActivateProjections());
            _coroutineRunner.Run(StartCooldown());
        }

        public void UpdatePlayerModifiers(IReadOnlyDictionary<BaseProperty, float> stats) =>
            _abilityData.UpdateHeroModifiers(stats);

        public void Upgrade()
        {
            _currentUpgradeLevel++;
            List<AbilityUpgradeData> upgrades = new List<AbilityUpgradeData>();

            for (int i = 0; i < _currentUpgradeLevel; i++)
                upgrades.Add(_abilityUpgradesData[i]);

            _abilityData.UpdateUpgradeModifiers(upgrades);
        }

        public void ResetUpgrades()
        {
            _currentUpgradeLevel = 0;
            _abilityData.UpdateUpgradeModifiers(new AbilityUpgradeData[] { });
        }

        private IEnumerator ActivateProjections()
        {
            for (int i = 0; i < _abilityData.BurstCount; i++)
            {
                _projectionBuilder.Build(_abilityData, _pivotObject, _projectionPool);
                yield return new WaitForSeconds(_abilityData.BurstFireDelay);
            }
        }

        private IEnumerator StartCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(_abilityData.Cooldown);
            _onCooldown = false;
        }

        public bool CheckConfig(AbilityConfigSO abilityConfigSO) =>
            _abilityUpgradesData[0].BaseConfigSO == abilityConfigSO;
    }
}