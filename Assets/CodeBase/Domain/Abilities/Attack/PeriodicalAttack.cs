using System;
using System.Collections;
using CodeBase.Domain.EntityComponents;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    [Serializable]
    public sealed class PeriodicalAttack : AttackBehaviour
    {
        private bool _canRun;

        public PeriodicalAttack(AbilityData abilityConfig, bool canRun) : base(abilityConfig)
        {
            _canRun = canRun;
        }

        public override IEnumerator Run()
        {
            yield return base.Run();

            while (_canRun)
            {
                CheckOverlap();

                yield return AttackDelayInSeconds;
            }
        }

        private void CheckOverlap()
        {
            int count = Rigidbody2D.Cast(Vector2.zero, AbilityConfig.WhatIsEnemy, Results);

            if (count == 0)
                return;

            for (var i = 0; i < count; i++)
            {
                if (Results[i].collider.TryGetComponent(out Damageable enemy) == false)
                    continue;

                InvokeEnemyHitted(enemy.transform);
                enemy.TakeDamage(AbilityConfig.Damage, AbilityConfig.Stagger);
            }
        }
    }
}