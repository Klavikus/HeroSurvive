using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Movement
{
    [Serializable]
    public sealed class MoveUp : MovementBehaviour
    {
        public override IEnumerator Run()
        {
            float elapsedTime = 0;
            ObjectForMove.position = SpawnData.StartPosition;

            if (AlignRotationWithDirection)
                ObjectForMove.up = SpawnData.NewDirection;

            if (FlipDirectionAllowed)
            {
                float currentScaleX = ObjectForMove.localScale.x;

                if (SpawnData.NewDirection.x < 0)
                {
                    currentScaleX = currentScaleX < 0 ? currentScaleX : -currentScaleX;
                    ObjectForMove.localScale = new Vector3(currentScaleX, ObjectForMove.localScale.y,
                        ObjectForMove.localScale.y);
                }
                else
                {
                    currentScaleX = currentScaleX > 0 ? currentScaleX : -currentScaleX;
                    ObjectForMove.localScale = new Vector3(currentScaleX, ObjectForMove.localScale.y,
                        ObjectForMove.localScale.y);
                }
            }

            while (elapsedTime < BaseDataConfig.Duration)
            {
                ObjectForMove.position += SpawnData.NewDirection * (BaseDataConfig.Speed * Time.deltaTime);
                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}