using System;
using System.Collections;
using CodeBase.GameCore.Domain.EntityComponents;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities.Attack
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

            if (count == 0)
                return;

            for (var i = 0; i < count; i++)
            {
                if (Results[i].collider.TryGetComponent(out Damageable damageable) == false)
                    continue;

                InvokeEnemyHitted(damageable.transform);
                damageable.TakeDamage(AbilityConfig.Damage, AbilityConfig.Stagger);
            }
        }
    }
}