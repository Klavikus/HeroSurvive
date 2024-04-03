using System.Collections;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop.Abilities
{
    public interface IAbilityAnimator
    {
        void Initialize(SpriteRenderer renderer);
        IEnumerator Run();
    }
}