using System.Collections;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Domain.Abilities.Movement
{
    public interface IMovementBehaviour
    {
        void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, ITargetService targetFinderService);

        IEnumerator Run();
    }
}