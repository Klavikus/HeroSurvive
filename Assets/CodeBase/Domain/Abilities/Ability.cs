using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Domain
{
    public class Ability : IDisposable
    {
        private readonly AbilityData _abilityData;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ProjectionPool _projectionPool;
        private readonly AbilityProjectionBuilder _projectionBuilder;
        private readonly AbilityUpgradeData[] _abilityUpgradesData;
        private readonly IGameLoopService _gameLoopService;

        private readonly WaitForSeconds _cooldownWaitForSeconds;
        private readonly WaitForSeconds _burstDelayWaitForSeconds;
        private readonly RaycastHit2D[] _raycastHits;

        private Transform _pivotObject;
        private bool _onCooldown;
        private bool _isInitialized;
        private int _currentUpgradeLevel;
        private Coroutine _activateProjectionCoroutine;
        private Coroutine _runCoroutine;

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
            _cooldownWaitForSeconds = new WaitForSeconds(_abilityData.Cooldown);
            _burstDelayWaitForSeconds = new WaitForSeconds(_abilityData.BurstFireDelay);
            _raycastHits = new RaycastHit2D[abilityData.CheckCount];
        }

        ~Ability() =>
            ReleaseUnmanagedResources();

        public bool CanUpgrade => _currentUpgradeLevel < _abilityUpgradesData.Length;
        public AbilityUpgradeData AvailableUpgrade => _abilityUpgradesData[_currentUpgradeLevel];

        public void Initialize(Transform pivotObject)
        {
            _pivotObject = pivotObject;
            _isInitialized = true;
        }

        public void Execute()
        {
            if (_onCooldown || !_isInitialized || !CheckForTargetExistence())
                return;

            if (_activateProjectionCoroutine != null)
                _coroutineRunner.Stop(_activateProjectionCoroutine);

            _activateProjectionCoroutine = _coroutineRunner.Run(ActivateProjections());

            if (_runCoroutine != null)
                _coroutineRunner.Stop(_runCoroutine);

            _runCoroutine = _coroutineRunner.Run(StartCooldown());
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

        public bool CheckConfig(AbilityConfigSO abilityConfigSO) =>
            _abilityUpgradesData[0].BaseConfigSO == abilityConfigSO;

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private void OnGameCloseInvoked()
        {
            _coroutineRunner.Stop(_activateProjectionCoroutine);
            _coroutineRunner.Stop(_runCoroutine);
            _projectionPool.Clear();
        }

        private bool CheckForTargetExistence()
        {
            return Physics2D.CircleCastNonAlloc(_pivotObject.position, _abilityData.EnemyCheckRadius, Vector2.zero,
                _raycastHits, 1, _abilityData.WhatIsEnemy.layerMask) > 0;
        }

        private IEnumerator ActivateProjections()
        {
            for (int i = 0; i < _abilityData.BurstCount; i++)
            {
                _projectionBuilder.Build(_abilityData, _pivotObject, _projectionPool);

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
            _gameLoopService.LevelCloseInvoked -= OnGameCloseInvoked;

            if (_activateProjectionCoroutine != null)
                _coroutineRunner.Stop(_activateProjectionCoroutine);

            if (_runCoroutine != null)
                _coroutineRunner.Stop(_runCoroutine);
        }
    }
}