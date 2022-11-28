using System.Collections;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Movement
{
    public abstract class MovementBehaviour : IMovementBehaviour
    {
        protected Transform ObjectForMove;
        protected AbilityData BaseDataConfig;
        protected float Offset;
        protected bool AlignRotationWithDirection;
        protected bool FlipDirectionAllowed;
        protected SpawnData SpawnData;
        protected TargetFinderService TargetFinderService;

        public void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, TargetFinderService targetFinderService)
        {
            SpawnData = spawnData;
            ObjectForMove = objectForMove;
            BaseDataConfig = abilityBaseDataConfig;
            Offset = spawnData.Offset;
            AlignRotationWithDirection = abilityBaseDataConfig.AlignWithRotation;
            FlipDirectionAllowed = abilityBaseDataConfig.FlipDirectionAllowed;
            TargetFinderService = targetFinderService;
        }

        public abstract IEnumerator Run();
    }
}