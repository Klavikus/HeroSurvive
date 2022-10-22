using System.Collections;
using UnityEngine;

namespace CodeBase.Abilities.Movement
{
    public abstract class MovementBehaviour : IMovementBehaviour
    {
        protected Transform _objectForMove;
        protected AbilityData _baseDataConfig;
        protected float _offset;
        protected int _id;
        protected bool _alignRotationWithDirection;
        protected SpawnData SpawnData;
        protected TargetService _targetService;

        public void Initialize(Transform objectForMove, SpawnData spawnData,
            AbilityData abilityBaseDataConfig, TargetService targetService)
        {
            SpawnData = spawnData;
            _objectForMove = objectForMove;
            _baseDataConfig = abilityBaseDataConfig;
            _offset = spawnData.Offset;
            _id = spawnData.Id;
            _alignRotationWithDirection = abilityBaseDataConfig.AlignWithRotation;
            _targetService = targetService;
        }

        public abstract IEnumerator Run();
    }
}