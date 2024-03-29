using System.Collections;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities.Size
{
    public class SizeOverLifetime : ISizeBehaviour
    {
        private const int StepsInSecond = 30;

        private Transform _targetTransform;
        private SizeBehaviourData _sizeBehaviourData;
        private WaitForSeconds _oneStepDuration;
        private WaitForSeconds _mainPeriodInSeconds;

        public void Initialize(Transform targetTransform, SizeBehaviourData sizeBehaviourData)
        {
            _targetTransform = targetTransform;
            _sizeBehaviourData = sizeBehaviourData;

            float startPeriod = _sizeBehaviourData.FullTimePeriod * _sizeBehaviourData.StartTime;
            float endPeriod = _sizeBehaviourData.FullTimePeriod * _sizeBehaviourData.EndTime;
            float mainPeriod = _sizeBehaviourData.FullTimePeriod - startPeriod - endPeriod;

            _oneStepDuration = new WaitForSeconds(1f / StepsInSecond);
            _mainPeriodInSeconds = new WaitForSeconds(mainPeriod);
        }

        public IEnumerator Run()
        {
            float currentSize = _sizeBehaviourData.StartTargetSize;

            float startPeriodDuration = _sizeBehaviourData.StartTime * _sizeBehaviourData.FullTimePeriod;
            float startPeriodSteps = startPeriodDuration * StepsInSecond;
            float startPeriodStepValue = (_sizeBehaviourData.MainTargetSize - _sizeBehaviourData.StartTargetSize) /
                                         startPeriodSteps;

            for (int i = 0; i < startPeriodSteps; i++)
            {
                currentSize += startPeriodStepValue;
                _targetTransform.localScale = currentSize * Vector3.one;

                yield return _oneStepDuration;
            }

            _targetTransform.localScale = _sizeBehaviourData.MainTargetSize * Vector3.one;

            yield return _mainPeriodInSeconds;

            float endPeriodDuration = _sizeBehaviourData.EndTime * _sizeBehaviourData.FullTimePeriod;
            float endPeriodSteps = endPeriodDuration * StepsInSecond;
            float endPeriodStepValue = (_sizeBehaviourData.EndTargetSize - _sizeBehaviourData.MainTargetSize) /
                                       endPeriodSteps;

            for (int i = 0; i < endPeriodSteps; i++)
            {
                currentSize += endPeriodStepValue;
                _targetTransform.localScale = currentSize * Vector3.one;

                yield return _oneStepDuration;
            }
        }
    }
}