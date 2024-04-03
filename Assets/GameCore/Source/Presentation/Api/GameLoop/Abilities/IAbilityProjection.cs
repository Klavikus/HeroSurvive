using System;
using System.Collections;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Presentation.Api.GameLoop.Abilities
{
    public interface IAbilityProjection
    {
        event Action<IAbilityProjection> Destroyed;
        bool UseVfx { get; }
        IAbilityAnimator Animator { get; }
        SpriteRenderer SpriteRenderer { get; }
        GameObject GameObject { get; }
        Rigidbody2D Rigidbody { get; }
        void Construct(IPresenter presenter);
        IEnumerator Run();
    }
}