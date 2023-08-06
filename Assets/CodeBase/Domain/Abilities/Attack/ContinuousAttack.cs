using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public sealed class ContinuousAttack : AttackBehaviour
    {
        public ContinuousAttack(AbilityData abilityConfig) : base(abilityConfig)
        {
        }

        public override IEnumerator Run()
        {
            yield return base.Run();

            while (CanRun)
            {
                CheckOverlap();
                yield return null;
            }
        }

        private void CheckOverlap()
        {
            int count = Rigidbody2D.Cast(Vector2.zero, AbilityConfig.WhatIsEnemy, Results);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                    if (Results[i].collider.TryGetComponent(out Damageable damageable) && damageable.CanReceiveDamage)
                        CurrentEnemies.Add(damageable);

                foreach (Damageable newDamageable in CurrentEnemies.Except(PreviousEnemies))
                {
                    newDamageable.TakeDamage(AbilityConfig.Damage, AbilityConfig.Stagger);
                    InvokeEnemyHitted(newDamageable.transform);
                    if (AbilityConfig.IsLimitedPenetration && --Penetration == 0)
                    {
                        CanRun = false;
                        InvokePenetrationLimit();
                    }
                }
            }

            PreviousEnemies.Clear();
            PreviousEnemies.AddRange(CurrentEnemies);
            CurrentEnemies.Clear();
        }
    }
}