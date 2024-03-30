using System.Collections;
using UnityEngine;

namespace GameCore.Source.Domain.Abilities.Size
{
    public interface ISizeBehaviour
    {
        void Initialize(Transform targetTransform, SizeBehaviourData sizeBehaviourData);
        IEnumerator Run();
    }
}