using System.Collections;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Movement
{
    public interface IMovementBehaviour
    {
        void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, ITargetService targetFinderService);

        IEnumerator Run();
    }
}