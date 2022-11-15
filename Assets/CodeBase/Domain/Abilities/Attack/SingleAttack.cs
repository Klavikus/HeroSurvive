using System;
using System.Collections;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    [Serializable]
    public sealed class SingleAttack : AttackBehaviour
    {
        public SingleAttack(AbilityData abilityConfig) : base(abilityConfig)
        {
        }

        public override IEnumerator Run()
        {
            yield return base.Run();
            CheckOverlap();
        }

        private void CheckOverlap()
        {
            int count = _rigidbody2D.Cast(Vector2.zero, _abilityConfig.WhatIsEnemy, _results);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                    if (_results[i].collider.TryGetComponent(out Damageable damageable))
                        damageable.TakeDamage(_abilityConfig.Damage);
            }
        }
    }
}