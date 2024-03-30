using System;
using System.Collections;
using GameCore.Source.Presentation.Api;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.Abilities
{
    public class AbilityProjection : MonoBehaviour, IAbilityProjection
    {
        [SerializeField] private AbilityAnimator _abilityAnimator;

        public event Action<IAbilityProjection> Destroyed;

        [field: SerializeField] public bool UseVfx { get; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; }
       
        public GameObject GameObject => gameObject;
        public IAbilityAnimator Animator => _abilityAnimator;

        public IEnumerator Run()
        {
            yield return _abilityAnimator.Run();
        }

        private void OnDestroy() => Destroyed?.Invoke(this);
    }
}