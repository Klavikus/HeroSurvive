using System.Collections;
using CodeBase.GameCore.Infrastructure.Factories;
using CodeBase.GameCore.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities.Movement
{
    public abstract class MovementBehaviour : IMovementBehaviour
    {
        public Transform ObjectForMove { get; private set; }
        public AbilityData BaseDataConfig { get; private set; }
        public float Offset { get; private set; }
        public bool AlignRotationWithDirection { get; private set; }
        public bool FlipDirectionAllowed { get; private set; }
        public SpawnData SpawnData { get; private set; }
        public ITargetService TargetFinderService { get; private set; }

        public void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, ITargetService targetFinderService)
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