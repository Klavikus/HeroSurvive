using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters.GameLoop;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Abilities.Attack;
using GameCore.Source.Domain.Abilities.Movement;
using GameCore.Source.Domain.Abilities.Size;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core.Pools;
using GameCore.Source.Presentation.Api.GameLoop.Abilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class AbilityProjectionBuilder
    {
        private readonly ITargetService _targetFinderService;
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IProjectionPool _projectionPool;
        private readonly Transform _poolContainer;

        private readonly Dictionary<MoveType, Type> _dictionaryMoveTypes = new()
        {
            [MoveType.Orbital] = typeof(OrbitalMove),
            [MoveType.OrbitalMovePoint] = typeof(OrbitalMovePoint),
            [MoveType.MoveUp] = typeof(MoveUp),
            [MoveType.Follow] = typeof(Follow),
        };

        private readonly Dictionary<AttackType, Type> _dictionaryAttackTypes = new()
        {
            [AttackType.Continuous] = typeof(ContinuousAttack),
            [AttackType.Periodical] = typeof(PeriodicalAttack),
            [AttackType.Single] = typeof(SingleAttack),
        };

        private readonly Dictionary<SizeType, Type> _dictionarySizeTypes = new()
        {
            [SizeType.Constant] = typeof(ConstantSize),
            [SizeType.OverLifetime] = typeof(SizeOverLifetime),
            [SizeType.OverLifetimeFixed] = typeof(SizeOverLifetimeFixed),
        };

        private readonly Dictionary<AbilityData, IProjectionPool> _projectionPools = new();

        public AbilityProjectionBuilder(
            ITargetService targetService,
            IAudioPlayerService audioPlayerService,
            ICoroutineRunner coroutineRunner)
        {
            _targetFinderService = targetService;
            _audioPlayerService = audioPlayerService;
            _coroutineRunner = coroutineRunner;
            _poolContainer = Object.Instantiate(new GameObject("poolContainer")).transform;
        }

        public IProjectionPool GetOrCreateProjectionPool(AbilityData abilityConfigSo)
        {
            if (_projectionPools.ContainsKey(abilityConfigSo))
            {
                _projectionPools[abilityConfigSo].Clear();

                return _projectionPools[abilityConfigSo];
            }

            IProjectionPool projectionPool = new ProjectionPool(_poolContainer, abilityConfigSo.AbilityView);
            _projectionPools.Add(abilityConfigSo, projectionPool);

            return projectionPool;
        }

        public IAbilityProjection[] Build(AbilityData abilityData, Transform pivotObject, IProjectionPool projectionPool)
        {
            IAbilityProjection[] projections = projectionPool.GetProjections(abilityData.SpawnCount);

            for (var i = 0; i < projections.Length; i++)
            {
                AbilityProjectionPresenter abilityProjectionPresenter = new(
                    projections[i],
                    abilityData,
                    _audioPlayerService,
                    _coroutineRunner,
                    projectionPool,
                    GetAttackBehaviour(abilityData),
                    GetMovementBehaviour(abilityData),
                    GetSizeBehaviour(abilityData),
                    _targetFinderService, 
                    GetSpawnData(abilityData, pivotObject, i));
                
                projections[i].Construct(abilityProjectionPresenter);
            }

            return projections;
        }

        private ISizeBehaviour GetSizeBehaviour(AbilityData abilityConfig)
        {
            Type sizeType = _dictionarySizeTypes.ContainsKey(abilityConfig.SizeBehaviourData.SizeType)
                ? _dictionarySizeTypes[abilityConfig.SizeBehaviourData.SizeType]
                : throw new NullReferenceException(nameof(sizeType));

            return Activator.CreateInstance(sizeType) as ISizeBehaviour;
        }

        private IAttackBehaviour GetAttackBehaviour(AbilityData abilityConfig)
        {
            Type attackType = _dictionaryAttackTypes.ContainsKey(abilityConfig.AttackType)
                ? _dictionaryAttackTypes[abilityConfig.AttackType]
                : throw new NullReferenceException(nameof(attackType));

            return Activator.CreateInstance(attackType, new object[] {abilityConfig}) as IAttackBehaviour;
        }

        private IMovementBehaviour GetMovementBehaviour(AbilityData abilityConfig)
        {
            Type movementType = _dictionaryMoveTypes.ContainsKey(abilityConfig.MoveType)
                ? _dictionaryMoveTypes[abilityConfig.MoveType]
                : throw new NullReferenceException(nameof(movementType));

            return Activator.CreateInstance(movementType) as IMovementBehaviour;
        }

        private SpawnData GetSpawnData(AbilityData abilityData, Transform pivotObject, int i)
        {
            Vector3 enemyPositionFromTargetService =
                _targetFinderService.GetClosestEnemyToPlayer(abilityData.AttackRadius,
                    abilityData.WhatIsEnemy.layerMask);
            Vector3 directionToClosest = (enemyPositionFromTargetService - pivotObject.position).normalized;
            Vector3 enemyPosition = Vector3.zero;

            if (abilityData.TargetingType == TargetingType.ByDirection)
                directionToClosest = _targetFinderService.GetPlayerDirection();

            if (abilityData.TargetingType == TargetingType.ToClosest)
            {
                enemyPosition = enemyPositionFromTargetService;

                directionToClosest = (enemyPositionFromTargetService - pivotObject.position).normalized;

                if (enemyPositionFromTargetService == Vector3.zero)
                    directionToClosest = _targetFinderService.GetPlayerDirection();
            }

            if (abilityData.TargetingType == TargetingType.RandomTarget)
            {
                enemyPosition = _targetFinderService.GetRandomEnemyPosition();
                directionToClosest = (enemyPosition - pivotObject.position).normalized;
                if (enemyPositionFromTargetService == Vector3.zero)
                    directionToClosest = _targetFinderService.GetPlayerDirection();
            }

            float distanceToTarget = (enemyPositionFromTargetService - pivotObject.position).magnitude;

            Vector3 startPosition = Vector3.zero;
            float newOffset = 0;

            switch (abilityData.SpawnPosition)
            {
                case SpawnType.Point:
                    // Vector3 startPosition = pivotObject.position + directionToClosest * abilityData.Radius;
                    startPosition = enemyPosition;
                    if (abilityData.MoveType == MoveType.Orbital)
                        newOffset = 360f / abilityData.SpawnCount * i;
                    else
                        newOffset = abilityData.Radius;

                    return new SpawnData(pivotObject, i, newOffset, startPosition, directionToClosest);

                case SpawnType.PivotPoint:
                    startPosition = pivotObject.position + directionToClosest * abilityData.Radius;
                    if (abilityData.MoveType == MoveType.Orbital)
                        newOffset = 360f / abilityData.SpawnCount * i;
                    else
                        newOffset = abilityData.Radius;

                    return new SpawnData(pivotObject, i, newOffset, startPosition, directionToClosest);

                case SpawnType.Circle:
                    float degreeStep = 360f / abilityData.SpawnCount;
                    float offset = degreeStep * i;
                    Vector3 newPosition = new Vector3(
                        Mathf.Cos(offset * Mathf.Deg2Rad) * abilityData.Radius + pivotObject.position.x,
                        Mathf.Sin(offset * Mathf.Deg2Rad) * abilityData.Radius + pivotObject.position.y,
                        0);
                    Vector3 newDirection = (newPosition - pivotObject.position).normalized;

                    return new SpawnData(pivotObject, i, offset, newPosition, newDirection);

                case SpawnType.Arc:
                    float arcDegrees = abilityData.Arc;
                    float arcStep = arcDegrees / abilityData.SpawnCount;
                    float arcOffset = arcStep / 2;
                    float degreeOffset = abilityData.Arc / 2 - arcOffset - arcStep * i;
                    Vector3 startPos = pivotObject.position +
                                       Quaternion.Euler(0, 0, degreeOffset) * directionToClosest *
                                       abilityData.Radius;
                    Vector3 newDir = (startPos - pivotObject.position).normalized;

                    return new SpawnData(pivotObject, i, abilityData.Radius, startPos, newDir);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}