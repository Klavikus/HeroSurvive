using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class AbilityController : IDisposable, IAbilityController
    {
        private readonly AbilityData _abilityData;
        private readonly AbilityUpgradeData[] _abilityUpgradesData;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IProjectionPool _projectionPool;
        private readonly AbilityProjectionBuilder _abilityProjectionBuilder;

        private readonly WaitForSeconds _cooldownWaitForSeconds;
        private readonly WaitForSeconds _burstDelayWaitForSeconds;
        private readonly RaycastHit2D[] _raycastHits;
        private readonly Transform _pivotObject;

        private bool _onCooldown;
        private bool _isInitialized;
        private int _currentUpgradeLevel;
        private Coroutine _activateProjectionCoroutine;
        private Coroutine _runCoroutine;

        public AbilityController(
            AbilityData abilityData,
            AbilityUpgradeData[] abilityUpgradesData,
            ICoroutineRunner coroutineRunner,
            IProjectionPool projectionPool,
            AbilityProjectionBuilder abilityProjectionBuilder,
            Transform pivotObject)
        {
            _abilityData = abilityData ?? throw new ArgumentNullException(nameof(abilityData));
            _abilityUpgradesData = abilityUpgradesData ?? throw new ArgumentNullException(nameof(abilityUpgradesData));

            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _projectionPool = projectionPool ?? throw new ArgumentNullException(nameof(projectionPool));
            _abilityProjectionBuilder = abilityProjectionBuilder ??
                                        throw new ArgumentNullException(nameof(abilityProjectionBuilder));
            _pivotObject = pivotObject ? pivotObject : throw new ArgumentNullException(nameof(pivotObject));
        }

        public void Execute()
        {
            if (_onCooldown || !_isInitialized || !CheckForTargetExistence())
                return;

            if (_activateProjectionCoroutine != null)
                _coroutineRunner.StopCoroutine(_activateProjectionCoroutine);

            _activateProjectionCoroutine = _coroutineRunner.StartCoroutine(ActivateProjections());

            if (_runCoroutine != null)
                _coroutineRunner.StopCoroutine(_runCoroutine);

            _runCoroutine = _coroutineRunner.StartCoroutine(StartCooldown());
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

        public bool CheckConfig(AbilityConfigSO abilityConfigSo) =>
            _abilityUpgradesData[0].BaseConfigSO == abilityConfigSo;

        public void Dispose() =>
            ReleaseUnmanagedResources();

        private bool CheckForTargetExistence() =>
            Physics2D.CircleCastNonAlloc(_pivotObject.position, _abilityData.EnemyCheckRadius, Vector2.zero,
                _raycastHits, 1, _abilityData.WhatIsEnemy.layerMask) > 0;

        private IEnumerator ActivateProjections()
        {
            for (int i = 0; i < _abilityData.BurstCount; i++)
            {
                _abilityProjectionBuilder.Build(_abilityData, _pivotObject, _projectionPool);

                yield return _burstDelayWaitForSeconds;
            }
        }

        private IEnumerator StartCooldown()
        {
            _onCooldown = true;

            yield return _cooldownWaitForSeconds;

            _onCooldown = false;
        }

        private void ReleaseUnmanagedResources()
        {
            if (_activateProjectionCoroutine != null)
                _coroutineRunner.StopCoroutine(_activateProjectionCoroutine);

            if (_runCoroutine != null)
                _coroutineRunner.StopCoroutine(_runCoroutine);
            
            _projectionPool.Clear();
        }
    }
}