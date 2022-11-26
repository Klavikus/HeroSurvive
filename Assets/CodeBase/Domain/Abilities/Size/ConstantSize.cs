using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Size
{
    public class ConstantSize : ISizeBehaviour
    {
        private Transform _targetTransform;
        private SizeBehaviourData _sizeBehaviourData;
        private Vector3 _targetLocalScale;

        public void Initialize(Transform targetTransform, SizeBehaviourData sizeBehaviourData)
        {
            _targetTransform = targetTransform;
            _sizeBehaviourData = sizeBehaviourData;
            _targetLocalScale = Vector3.one * _sizeBehaviourData.MainTargetSize;
        }

        public IEnumerator Run()
        {
            _targetTransform.localScale = _targetLocalScale;
            yield break;
        }
    }
}