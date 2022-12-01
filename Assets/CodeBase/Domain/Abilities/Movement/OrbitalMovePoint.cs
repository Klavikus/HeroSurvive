using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Movement
{
    [Serializable]
    public sealed class OrbitalMovePoint : MovementBehaviour
    {
        private float _currenRotationAngle;

        public override IEnumerator Run()
        {
            _currenRotationAngle =  Offset * Mathf.Deg2Rad;

            float startDuration = BaseDataConfig.Duration * BaseDataConfig.StartTimePercent;
            float midDuration = BaseDataConfig.Duration - BaseDataConfig.Duration *
                (BaseDataConfig.StartTimePercent + BaseDataConfig.EndTimePercent);
            float endDuration = BaseDataConfig.Duration * BaseDataConfig.EndTimePercent;

            yield return MoveStage(startDuration, BaseDataConfig.StartRadiusCurve);
            yield return MoveStage(midDuration, BaseDataConfig.MainRadiusCurve);
            yield return MoveStage(endDuration, BaseDataConfig.EndRadiusCurve);
        }

        private IEnumerator MoveStage(float duration, AnimationCurve stageCurve)
        {
            float elapsedTime = 0;

            while (elapsedTime <= duration)
            {
                float currentPercentTime = elapsedTime / duration;

                _currenRotationAngle += BaseDataConfig.RotationStep * BaseDataConfig.Speed * Time.deltaTime *
                                        Mathf.Deg2Rad;

                Vector3 newPosition = new Vector3(
                    Mathf.Cos(_currenRotationAngle) * stageCurve.Evaluate(currentPercentTime) * BaseDataConfig.Radius +
                    SpawnData.StartPosition.x,
                    Mathf.Sin(_currenRotationAngle) * stageCurve.Evaluate(currentPercentTime) * BaseDataConfig.Radius +
                    SpawnData.StartPosition.y,
                    0);

                if (AlignRotationWithDirection)
                    ObjectForMove.up = newPosition - TargetFinderService.GetPlayerPosition();

                ObjectForMove.position = newPosition;

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}