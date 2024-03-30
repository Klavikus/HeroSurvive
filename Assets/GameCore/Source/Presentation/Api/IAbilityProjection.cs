using System;
using System.Collections;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface IAbilityProjection
    {
        event Action<IAbilityProjection> Destroyed;
        bool UseVfx { get; }
        IAbilityAnimator Animator { get; }
        SpriteRenderer SpriteRenderer { get; }
        GameObject GameObject { get; }
        IEnumerator Run();
    }
}