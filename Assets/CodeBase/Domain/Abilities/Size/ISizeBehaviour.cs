using System.Collections;
using UnityEngine;

namespace CodeBase.Domain
{
    public interface ISizeBehaviour
    {
        void Initialize(Transform targetTransform, SizeBehaviourData sizeBehaviourData);
        IEnumerator Run();
    }
}