using System.Collections;
using UnityEngine;

namespace CodeBase.Abilities.Movement
{
    public interface IMovementBehaviour
    {
        void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, TargetService targetService);

        IEnumerator Run();
    }
}