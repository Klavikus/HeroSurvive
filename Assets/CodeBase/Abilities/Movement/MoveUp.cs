using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Abilities.Movement
{
    [Serializable]
    public sealed class MoveUp : MovementBehaviour
    {
        private Transform _currentMoveObject;

        public override IEnumerator Run()
        {
            float elapsedTime = 0;
            _currentMoveObject = _objectForMove.transform;

            _objectForMove.position = SpawnData.StartPosition;
            _objectForMove.up = SpawnData.NewDirection;

            while (elapsedTime < _baseDataConfig.Duration)
            {
                _currentMoveObject.position += _currentMoveObject.up * (_baseDataConfig.Speed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}