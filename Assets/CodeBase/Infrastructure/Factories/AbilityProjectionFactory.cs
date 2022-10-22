using System;
using System.Collections.Generic;
using CodeBase.Abilities;
using CodeBase.Abilities.Attack;
using CodeBase.Abilities.Movement;
using UnityEngine;

namespace CodeBase
{
    public class AbilityProjectionFactory
    {
        private readonly TargetService _targetService;
        private readonly Transform _poolContainer;

        private Dictionary<MoveType, Type> _dictionaryMoveTypes = new Dictionary<MoveType, Type>()
        {
            [MoveType.Orbital] = typeof(OrbitalMove),
            [MoveType.MoveUp] = typeof(MoveUp),
        };

        private Dictionary<AttackType, Type> _dictionaryAttackTypes = new Dictionary<AttackType, Type>()
        {
            [AttackType.Continuous] = typeof(ContinuousAttack),
            [AttackType.Periodical] = typeof(PeriodicalAttack),
            [AttackType.Single] = typeof(SingleAttack),
        };

        private readonly Dictionary<AbilityData, ProjectionPool> _projectionPools =
            new Dictionary<AbilityData, ProjectionPool>();

        public AbilityProjectionFactory(TargetService targetService, Transform poolContainer)
        {
            _targetService = targetService;
            _poolContainer = poolContainer;
        }

        public IAttackBehaviour GetAttackBehaviour(AbilityData abilityConfig)
        {
            Type attackType = _dictionaryAttackTypes.ContainsKey(abilityConfig.AttackType)
                ? _dictionaryAttackTypes[abilityConfig.AttackType]
                : throw new NullReferenceException(nameof(attackType));

            return Activator.CreateInstance(attackType, new object[] {abilityConfig}) as IAttackBehaviour;
        }

        public IMovementBehaviour GetMovementBehaviour(AbilityData abilityConfig)
        {
            Type movementType = _dictionaryMoveTypes.ContainsKey(abilityConfig.MoveType)
                ? _dictionaryMoveTypes[abilityConfig.MoveType]
                : throw new NullReferenceException(nameof(movementType));

            return Activator.CreateInstance(movementType) as IMovementBehaviour;
        }

        public SpawnData GetSpawnData(AbilityData abilityConfigSo, Transform pivotObject, int i)
        {
            Vector3 directionToTarget = _targetService.GetClosestEnemyToPlayer();
            Vector3 directionToClosest = (directionToTarget - pivotObject.position).normalized;
            if (directionToTarget == Vector3.zero)
            {
                directionToClosest = _targetService.GetPlayerDirection();
            }

            switch (abilityConfigSo.SpawnPosition)
            {
                case SpawnType.Point:
                    Vector3 startPosition = pivotObject.position + directionToClosest * abilityConfigSo.Radius;
                    float newOffset;
                    if (abilityConfigSo.MoveType == MoveType.Orbital)
                        newOffset = 360f / abilityConfigSo.SpawnCount * i;
                    else
                        newOffset = abilityConfigSo.Radius;
                    return new SpawnData(pivotObject, i, newOffset, startPosition,
                        directionToClosest);
               
                case SpawnType.Circle:
                    float degreeStep = 360f / abilityConfigSo.SpawnCount;
                    float offset = degreeStep * i;
                    Vector3 newPosition = new Vector3(
                        Mathf.Cos(offset * Mathf.Deg2Rad) * abilityConfigSo.Radius + pivotObject.position.x,
                        Mathf.Sin(offset * Mathf.Deg2Rad) * abilityConfigSo.Radius + pivotObject.position.y,
                        0);
                    Vector3 newDirection = (newPosition - pivotObject.position).normalized;
                    return new SpawnData(pivotObject, i, offset, newPosition, newDirection);
               
                case SpawnType.Arc:
                    float arcDegrees = abilityConfigSo.Arc;
                    float arcStep = arcDegrees / abilityConfigSo.SpawnCount;
                    float arcOffset = arcStep / 2;
                    float degreeOffset = abilityConfigSo.Arc / 2 - arcOffset - arcStep * i;
                    Vector3 startPos = pivotObject.position +
                                       Quaternion.Euler(0, 0, degreeOffset) * directionToClosest *
                                       abilityConfigSo.Radius;
                    Vector3 newDir = (startPos - pivotObject.position).normalized;
                    return new SpawnData(pivotObject, i, abilityConfigSo.Radius, startPos, newDir);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ProjectionPool CreateProjectionPool(AbilityData abilityConfigSo)
        {
            ProjectionPool projectionPool = new ProjectionPool(_poolContainer, abilityConfigSo.AbilityView);
            _projectionPools.Add(abilityConfigSo, projectionPool);
            return projectionPool;
        }
    }

    public struct SpawnData
    {
        public SpawnData(Transform pivotObject, int id, float offset, Vector3 startPosition, Vector3 newDirection)
        {
            Id = id;
            Offset = offset;
            StartPosition = startPosition;
            NewDirection = newDirection;
            PivotObject = pivotObject;
        }

        public Transform PivotObject { get; }
        public int Id { get; }
        public float Offset { get; }
        public Vector3 StartPosition { get; }
        public Vector3 NewDirection { get; }
    }
}