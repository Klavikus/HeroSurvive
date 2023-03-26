using System.Collections;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Domain
{
    public interface IMovementBehaviour
    {
        void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, ITargetService targetFinderService);

        IEnumerator Run();
    }
}