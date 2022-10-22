using System.Collections;
using System.Collections.Generic;
using CodeBase;
using CodeBase.Abilities;
using CodeBase.Stats;
using UnityEngine;

public class Ability
{
    private readonly TargetService _targetService;
    private AbilityData _abilityConfig;
    private CoroutineRunner _coroutineRunner;
    private ProjectionPool _projectionPool;
    private Transform _pivotObject;
    private bool _onCooldown;
    private AbilityProjectionFactory _projectionFactory;

    private bool _IsInitialized;

    public Ability(AbilityProjectionFactory projectionFactory, AbilityData abilityConfig,
        CoroutineRunner coroutineRunner, ProjectionPool projectionPool, TargetService targetService)
    {
        _projectionFactory = projectionFactory;
        _abilityConfig = abilityConfig;
        _coroutineRunner = coroutineRunner;
        _projectionPool = projectionPool;
        _targetService = targetService;
    }

    public void Initialize(Transform pivotObject)
    {
        _pivotObject = pivotObject;
        _IsInitialized = true;
    }

    public void Execute()
    {
        if (_onCooldown || !_IsInitialized)
            return;

        _coroutineRunner.Run(ActivateProjections());
        _coroutineRunner.Run(StartCooldown());
    }

    public void Update(IReadOnlyDictionary<PlayerStat, float> stats)
    {
        Debug.Log($"ab config {_abilityConfig.SpawnCount}");
        _abilityConfig.UseModifiers(stats);
        Debug.Log($"ab config after {_abilityConfig.SpawnCount}");
    }

    private IEnumerator ActivateProjections()
    {
        for (int i = 0; i < _abilityConfig.BurstCount; i++)
        {
            AbilityProjection[] projections = _projectionPool.GetProjections(_abilityConfig.SpawnCount);

            for (var j = 0; j < projections.Length; j++)
            {
                AbilityProjection abilityProjection = projections[j];

                abilityProjection.Initialize(_targetService,
                    _abilityConfig,
                    _projectionFactory.GetAttackBehaviour(_abilityConfig),
                    _projectionFactory.GetMovementBehaviour(_abilityConfig),
                    _projectionFactory.GetSpawnData(_abilityConfig, _pivotObject, j));
            }

            yield return new WaitForSeconds(_abilityConfig.BurstFireDelay);
        }
    }

    private IEnumerator StartCooldown()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(_abilityConfig.Cooldown);
        _onCooldown = false;
    }
}