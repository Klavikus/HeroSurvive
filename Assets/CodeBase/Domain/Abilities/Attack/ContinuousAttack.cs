using System;
using System.Collections;
using System.Linq;
using CodeBase.Domain.EntityComponents;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    [Serializable]
    public sealed class ContinuousAttack : AttackBehaviour
    {
        private bool _canRun;

        public ContinuousAttack(AbilityData abilityConfig) : base(abilityConfig)
        {
            _canRun = true;
        }

        public override IEnumerator Run()
        {
            yield return base.Run();

            while (_canRun)
            {
                CheckOverlap();

                yield return null;
            }
        }

        private void CheckOverlap()
        {
            int count = Rigidbody2D.Cast(Vector2.zero, AbilityConfig.WhatIsEnemy, Results);

            if (count == 0)
                return;

            for (var i = 0; i < count; i++)
                if (Results[i].collider.TryGetComponent(out Damageable damageable) && damageable.CanReceiveDamage)
                    CurrentEnemies.Add(damageable);

            foreach (Damageable newDamageable in CurrentEnemies.Except(PreviousEnemies))
            {
                newDamageable.TakeDamage(AbilityConfig.Damage, AbilityConfig.Stagger);
                InvokeEnemyHitted(newDamageable.transform);

                HandlePenetration();

                if (AbilityConfig.IsLimitedPenetration && Penetration == 0)
                {
                    _canRun = false;
                    InvokePenetrationLimit();
                }
            }

            PreviousEnemies.Clear();
            PreviousEnemies.AddRange(CurrentEnemies);
            CurrentEnemies.Clear();
        }
    }
}