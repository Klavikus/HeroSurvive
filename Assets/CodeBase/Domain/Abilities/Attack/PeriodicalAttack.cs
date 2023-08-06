using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public sealed class PeriodicalAttack : AttackBehaviour
    {
        public PeriodicalAttack(AbilityData abilityConfig) : base(abilityConfig)
        {
        }

        public override IEnumerator Run()
        {
            yield return base.Run();

            while (CanRun)
            {
                CheckOverlap();
                yield return AttackDelayInSeconds;
            }
        }

        private void CheckOverlap()
        {
            int count = Rigidbody2D.Cast(Vector2.zero, AbilityConfig.WhatIsEnemy, Results);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    if (Results[i].collider.TryGetComponent(out Damageable enemy))
                    {
                        InvokeEnemyHitted(enemy.transform);
                        enemy.TakeDamage(AbilityConfig.Damage, AbilityConfig.Stagger);
                    }
                }
            }
        }
    }
}