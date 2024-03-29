using System.Collections;
using CodeBase.GameCore.Infrastructure.Factories;
using CodeBase.GameCore.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities.Movement
{
    public interface IMovementBehaviour
    {
        void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, ITargetService targetFinderService);

        IEnumerator Run();
    }
}