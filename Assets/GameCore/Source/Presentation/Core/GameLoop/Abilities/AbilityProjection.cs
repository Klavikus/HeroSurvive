using System;
using System.Collections;
using GameCore.Source.Presentation.Api.GameLoop.Abilities;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.GameLoop.Abilities
{
    public class AbilityProjection : ViewBase, IAbilityProjection
    {
        [SerializeField] private AbilityAnimator _abilityAnimator;

        public event Action<IAbilityProjection> Destroyed;

        [field: SerializeField] public bool UseVfx { get; private set; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

        public GameObject GameObject => gameObject;
        public IAbilityAnimator Animator => _abilityAnimator;

        public IEnumerator Run()
        {
            yield return _abilityAnimator.Run();
        }

        protected override void OnBeforeDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}