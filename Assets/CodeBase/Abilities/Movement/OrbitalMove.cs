using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Abilities.Movement
{
    [Serializable]
    public sealed class OrbitalMove : MovementBehaviour
    {
        private float _currenRotationAngle;

        public override IEnumerator Run()
        {
            _currenRotationAngle =  _offset * Mathf.Deg2Rad;

            float elapsedTime = 0;
            float startDuration = _baseDataConfig.Duration * _baseDataConfig.StartTimePercent;
            float midDuration = _baseDataConfig.Duration - _baseDataConfig.Duration *
                (_baseDataConfig.StartTimePercent + _baseDataConfig.EndTimePercent);
            float endDuration = _baseDataConfig.Duration * _baseDataConfig.EndTimePercent;

            yield return MoveStage(startDuration, _baseDataConfig.StartRadiusCurve);
            yield return MoveStage(midDuration, _baseDataConfig.MainRadiusCurve);
            yield return MoveStage(endDuration, _baseDataConfig.EndRadiusCurve);
        }

        private IEnumerator MoveStage(float duration, AnimationCurve stageCurve)
        {
            float elapsedTime = 0;

            while (elapsedTime <= duration)
            {
                float currentPercentTime = elapsedTime / duration;

                _currenRotationAngle += _baseDataConfig.RotationStep * _baseDataConfig.Speed * Time.deltaTime *
                                        Mathf.Deg2Rad;

                Vector3 newPosition = new Vector3(
                    Mathf.Cos(_currenRotationAngle) * stageCurve.Evaluate(currentPercentTime) * _baseDataConfig.Radius +
                    SpawnData.PivotObject.position.x,
                    Mathf.Sin(_currenRotationAngle) * stageCurve.Evaluate(currentPercentTime) * _baseDataConfig.Radius +
                    SpawnData.PivotObject.position.y,
                    0);

                if (_alignRotationWithDirection)
                {
                    _objectForMove.up = newPosition - _targetService.GetPlayerPosition();
                }

                _objectForMove.position = newPosition;

                elapsedTime += Time.deltaTime;

                yield return null;
            }
        }
    }
}