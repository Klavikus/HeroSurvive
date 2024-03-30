using System;
using System.Collections;
using Modules.MVPPassiveView.Runtime;
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
        void Construct(IPresenter presenter);
        IEnumerator Run();
    }
}