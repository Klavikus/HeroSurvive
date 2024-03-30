using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Infrastructure.Api;
using JetBrains.Annotations;
using UnityEngine;
using ICoroutineRunner = GameCore.Source.Infrastructure.Api.Services.ICoroutineRunner;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class AbilityController : IDisposable, IAbilityController
    {
        private readonly AbilityData _abilityData;
        private readonly AbilityUpgradeData[] _abilityUpgradesData;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IProjectionPool _projectionPool;
        private readonly IGameLoopService _gameLoopService;
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
            [NotNull] AbilityData abilityData,
            [NotNull] AbilityUpgradeData[] abilityUpgradesData,
            [NotNull] ICoroutineRunner coroutineRunner,
            [NotNull] IProjectionPool projectionPool,
            [NotNull] IGameLoopService gameLoopService,
            [NotNull] AbilityProjectionBuilder abilityProjectionBuilder,
            [NotNull] Transform pivotObject)
        {
            _abilityData = abilityData ?? throw new ArgumentNullException(nameof(abilityData));
            _abilityUpgradesData = abilityUpgradesData ?? throw new ArgumentNullException(nameof(abilityUpgradesData));

            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _projectionPool = projectionPool ?? throw new ArgumentNullException(nameof(projectionPool));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _abilityProjectionBuilder = abilityProjectionBuilder ?? throw new ArgumentNullException(nameof(abilityProjectionBuilder));
            _pivotObject = pivotObject ?? throw new ArgumentNullException(nameof(pivotObject));

            _gameLoopService.LevelCloseInvoked += OnLevelCloseInvoked;
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

        public bool CheckConfig(AbilityConfigSO abilityConfigSO) =>
            _abilityUpgradesData[0].BaseConfigSO == abilityConfigSO;

        public void Dispose()
        {
            ReleaseUnmanagedResources();
        }

        private void OnLevelCloseInvoked()
        {
            throw new NotImplementedException();
        }

        private void OnGameCloseInvoked()
        {
            _coroutineRunner.StopCoroutine(_activateProjectionCoroutine);
            _coroutineRunner.StopCoroutine(_runCoroutine);
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
            _gameLoopService.LevelCloseInvoked -= OnGameCloseInvoked;

            if (_activateProjectionCoroutine != null)
                _coroutineRunner.StopCoroutine(_activateProjectionCoroutine);

            if (_runCoroutine != null)
                _coroutineRunner.StopCoroutine(_runCoroutine);
        }
    }
}