using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Size
{
    public class SizeOverLifetimeFixed : ISizeBehaviour
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

            float startPeriod = _sizeBehaviourData.StartTime;
            float endPeriod = _sizeBehaviourData.EndTime;
            float mainPeriod = _sizeBehaviourData.FullTimePeriod - startPeriod - endPeriod;

            _oneStepDuration = new WaitForSeconds(1f / StepsInSecond);
            _mainPeriodInSeconds = new WaitForSeconds(mainPeriod);
        }

        public IEnumerator Run()
        {
            float currentSize = _sizeBehaviourData.StartTargetSize;

            float startPeriodSteps = _sizeBehaviourData.StartTime * StepsInSecond;
            float startPeriodStepValue = (_sizeBehaviourData.MainTargetSize - _sizeBehaviourData.StartTargetSize) /
                                         startPeriodSteps;

            for (int i = 0; i < startPeriodSteps; i++)
            {
                currentSize += startPeriodStepValue;
                if (_targetTransform.localScale.x < 0)
                    _targetTransform.localScale = currentSize * new Vector3(-1, 1, 1);
                else
                    _targetTransform.localScale = currentSize * Vector3.one;

                yield return _oneStepDuration;
            }

            if (_targetTransform.localScale.x < 0)
                _targetTransform.localScale = _sizeBehaviourData.MainTargetSize * new Vector3(-1, 1, 1);
            else
                _targetTransform.localScale = _sizeBehaviourData.MainTargetSize * Vector3.one;

            yield return _mainPeriodInSeconds;

            float endPeriodSteps = _sizeBehaviourData.EndTime * StepsInSecond;
            float endPeriodStepValue = (_sizeBehaviourData.EndTargetSize - _sizeBehaviourData.MainTargetSize) /
                                       endPeriodSteps;

            for (int i = 0; i < endPeriodSteps; i++)
            {
                currentSize += endPeriodStepValue;
                if (_targetTransform.localScale.x < 0)
                    _targetTransform.localScale = currentSize * new Vector3(-1, 1, 1);
                else
                    _targetTransform.localScale = currentSize * Vector3.one;

                yield return _oneStepDuration;
            }
        }
    }
}