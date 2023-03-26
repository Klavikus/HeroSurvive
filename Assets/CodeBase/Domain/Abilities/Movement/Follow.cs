using System.Collections;
using UnityEngine;

namespace CodeBase.Domain
{
    public sealed class Follow : MovementBehaviour
    {
        private Transform _currentMoveObject;

        public override IEnumerator Run()
        {
            float elapsedTime = 0;
            _currentMoveObject = ObjectForMove.transform;
            ObjectForMove.position = SpawnData.StartPosition;

            if (AlignRotationWithDirection) 
                ObjectForMove.up = SpawnData.NewDirection;

            if (FlipDirectionAllowed)
            {
                float currentScaleX = _currentMoveObject.localScale.x;
                if (SpawnData.NewDirection.x < 0)
                {
                    currentScaleX = currentScaleX < 0 ? currentScaleX : -currentScaleX;
                    _currentMoveObject.localScale = new Vector3(currentScaleX, _currentMoveObject.localScale.y,
                        _currentMoveObject.localScale.y);
                }
                else
                {
                    currentScaleX = currentScaleX > 0 ? currentScaleX : -currentScaleX;
                    _currentMoveObject.localScale = new Vector3(currentScaleX, _currentMoveObject.localScale.y,
                        _currentMoveObject.localScale.y);
                }
            }

            while (elapsedTime < BaseDataConfig.Duration)
            {
                _currentMoveObject.position = SpawnData.PivotObject.position;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}