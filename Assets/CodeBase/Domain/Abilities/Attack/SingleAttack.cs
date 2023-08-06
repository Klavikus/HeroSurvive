using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Domain
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
            int count = Rigidbody2D.Cast(Vector2.zero, AbilityConfig.WhatIsEnemy, Results);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    if (Results[i].collider.TryGetComponent(out Damageable damageable))
                    {
                        InvokeEnemyHitted(damageable.transform);
                        damageable.TakeDamage(AbilityConfig.Damage, AbilityConfig.Stagger);
                    }
                }
            }
        }
    }
}