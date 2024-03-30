using System.Collections;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface IAbilityAnimator
    {
        void Initialize(SpriteRenderer renderer);
        IEnumerator Run();
    }
}