using System.Collections;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities.Size
{
    public interface ISizeBehaviour
    {
        void Initialize(Transform targetTransform, SizeBehaviourData sizeBehaviourData);
        IEnumerator Run();
    }
}